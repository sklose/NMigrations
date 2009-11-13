/*
 *  Licensed to the Apache Software Foundation (ASF) under one or more
 *  contributor license agreements.  See the NOTICE file distributed with
 *  this work for additional information regarding copyright ownership.
 *  The ASF licenses this file to You under the Apache License, Version 2.0
 *  (the "License"); you may not use this file except in compliance with
 *  the License.  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 *
 */

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Transactions;

using NMigrations.Repository;
using NMigrations.Sql;

namespace NMigrations.Core
{
    /// <summary>
    /// Implements the core engine that performs the migration progress.
    /// </summary>
    public class Engine
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Engine"/> class.
        /// </summary>
        public Engine()
        {
            Migrations = new Dictionary<long, Type>();
            SqlProviderFactory = new SqlProviderFactory();
            HistoryRepository = new MigrationHistoryRepository();
            SqlProcessor = new DatabaseSqlProcessor();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Migrates the database to the latest version.
        /// </summary>
        public void Migrate()
        {
            if (Migrations.Keys.Count > 0)
            {
                Migrate(Migrations.Keys.Max());
            }
        }

        /// <summary>
        /// Migrates the database to the specified <paramref name="version"/>.
        /// </summary>
        /// <param name="version">The version.</param>
        public void Migrate(long version)
        {
            //
            // Create a new context for the migration
            //
            var context = CreateContext();

            //
            // Find migrations that already have been applied
            //
            var history = HistoryRepository.RetrieveHistory(context);

            //
            // Find the migration path
            //
            var migrationPath = BuildMigrationPath(Migrations, history, version);

            //
            // Apply migrations
            //
            while (migrationPath.Count > 0)
            {
                var item = migrationPath.Dequeue();
                using (var txScope = new TransactionScope(TransactionScopeOption.Required, GetTransactionOptions()))
                {
                    // Connection was created before transaction scope
                    // --> need to enlist the tranaction manually
                    context.Connection.EnlistTransaction(Transaction.Current);

                    // Migrate
                    var migration = GetMigration(item.Key);
                    ApplyMigration(migration, item.Value, context);

                    // Persist changes
                    txScope.Complete();
                }
            }
        }

        /// <summary>
        /// Scans the specified <paramref name="assembly"/> for migrations and SQL providers.
        /// </summary>
        /// <param name="assembly">The assembly to add.</param>
        /// <returns>The engine.</returns>
        public virtual Engine AddAssembly(Assembly assembly)
        {
            FindMigrations(assembly);
            SqlProviderFactory.RegisterProviders(assembly);
            return this;
        }

        /// <summary>
        /// Registers the provider of the specified <see cref="Type"/> at
        /// the current <see cref="ProviderFactory"/>.
        /// </summary>
        /// <param name="providerType">Type of the provider.</param>
        /// <returns>The engine.</returns>
        public virtual Engine RegisterProvider(Type providerType)
        {
            SqlProviderFactory.RegisterProvider(providerType);
            return this;
        }

        /// <summary>
        /// Configures the database connection with the specified <paramref name="providerName"/>
        /// and the specified <paramref name="connectionString"/>.
        /// </summary>
        /// <param name="providerName">The invariant provider name.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>The engine.</returns>
        public virtual Engine SetConnectionString(string providerInvariantName, string connectionString)
        {
            ProviderInvariantName = providerInvariantName;
            ConnectionString = connectionString;
            return this;
        }

        /// <summary>
        /// Sets the SQL provider factory.
        /// </summary>
        /// <param name="factory">The SQL provider factory.</param>
        /// <returns>The engine.</returns>
        public virtual Engine SetSqlProviderFactory(SqlProviderFactory factory)
        {
            SqlProviderFactory = factory;
            return this;
        }

        /// <summary>
        /// Sets the SQL processor.
        /// </summary>
        /// <param name="processor">The processor.</param>
        /// <returns>The engine.</returns>
        public virtual Engine SetSqlProcessor(ISqlProcessor processor)
        {
            SqlProcessor = processor;
            return this;
        }

        /// <summary>
        /// Sets the migration history repository.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <returns>The engine.</returns>
        public virtual Engine SetMigrationHistoryRepository(IMigrationHistoryRepository repository)
        {
            HistoryRepository = repository;
            return this;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the invariant name of the provider.
        /// </summary>
        /// <value>The invariant name of the provider.</value>
        public string ProviderInvariantName
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the provider factory.
        /// </summary>
        /// <value>The provider factory.</value>
        public SqlProviderFactory SqlProviderFactory
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the migration history repository.
        /// </summary>
        /// <value>The history repository.</value>
        public IMigrationHistoryRepository HistoryRepository
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the SQL processor.
        /// </summary>
        /// <value>The SQL processor.</value>
        public ISqlProcessor SqlProcessor
        {
            get;
            protected set;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Creates a new migration context.
        /// </summary>
        /// <returns>The context.</returns>
        protected virtual MigrationContext CreateContext()
        {
            return new MigrationContext()
            {
                ConnetionString = ConnectionString,
                ProviderInvariantName = ProviderInvariantName,
                Connection = OpenConnection(),
                SqlProvider = SqlProviderFactory.GetProvider(ProviderInvariantName),
                SqlProcessor = SqlProcessor
            };
        }

        /// <summary>
        /// Opens up a new database connection.
        /// </summary>
        /// <returns>The database connection.</returns>
        protected virtual DbConnection OpenConnection()
        {
            var factory = DbProviderFactories.GetFactory(ProviderInvariantName);
            var connection = factory.CreateConnection();
            connection.ConnectionString = ConnectionString;
            connection.Open();

            return connection;
        }

        /// <summary>
        /// Builds the migration path from a specified database state (<paramref name="migrationHistory"/>)
        /// to the <paramref name="desiredVersion"/>.
        /// </summary>
        /// <param name="availableMigrations">The available migrations.</param>
        /// <param name="migrationHistory">The migration history.</param>
        /// <param name="desiredVersion">The desired version.</param>
        /// <returns>The queue that describes the migration path.</returns>
        protected virtual Queue<KeyValuePair<long, MigrationDirection>> BuildMigrationPath(IDictionary<long, Type> availableMigrations, IEnumerable<MigrationHistoryItem> migrationHistory, long desiredVersion)
        {
            //
            // Sort history by date
            //
            migrationHistory = migrationHistory.OrderByDescending(mh => mh.Date);

            //
            // Build up list of states of migrations
            //
            var migrationStates = new Dictionary<long, MigrationDirection>();
            foreach (var item in migrationHistory)
            {
                if (!migrationStates.ContainsKey(item.Version))
                {
                    migrationStates.Add(item.Version, item.Direction);
                }
            }

            //
            // Find migrations to apply
            //
            var downs = new List<KeyValuePair<long, Type>>();
            var ups = new List<KeyValuePair<long, Type>>();
            foreach (long version in availableMigrations.Keys.OrderByDescending(key => key))
            {
                if (version > desiredVersion)
                {
                    //
                    // Undo newer versions
                    //
                    if (migrationStates.ContainsKey(version) &&
                        migrationStates[version] == MigrationDirection.Up)
                    {
                        downs.Add(new KeyValuePair<long, Type>(version, availableMigrations[version]));
                    }
                }
                else
                {
                    //
                    // Apply older versions
                    //
                    if (!migrationStates.ContainsKey(version) ||
                        (migrationStates.ContainsKey(version) &&
                         migrationStates[version] == MigrationDirection.Down))
                    {
                        ups.Add(new KeyValuePair<long,Type>(version, availableMigrations[version]));
                    }
                }
            }

            //
            // Build final queue
            //
            var result = new Queue<KeyValuePair<long, MigrationDirection>>();
            foreach (var item in downs.OrderByDescending(kvp => kvp.Key))
	        {
                result.Enqueue(new KeyValuePair<long, MigrationDirection>(item.Key, MigrationDirection.Down));
	        }

            foreach (var item in ups.OrderBy(kvp => kvp.Key))
	        {
                result.Enqueue(new KeyValuePair<long, MigrationDirection>(item.Key, MigrationDirection.Up));
	        }

            return result;
        }

        /// <summary>
        /// Determines whether the specified <paramref name="type"/> is a valid implementation of <see cref="IMigration"/>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// 	<c>true</c> if the specified type is a valid migration; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool IsMigration(Type type)
        {
            return /* must implement IMigratoin */
                   type.GetInterfaces().Contains(typeof(IMigration)) &&

                   /* must have a Migration attribute */
                   type.GetCustomAttributes(typeof(MigrationAttribute), false).Length == 1 &&

                   /* must be instantiatable */
                   !type.IsAbstract &&

                   /* must have a default constructor */
                   type.GetConstructors().Any(ctor => ctor.IsPublic && ctor.GetParameters().Length == 0);
        }

        /// <summary>
        /// Finds all valid implementations of <see cref="IMigration"/> in the
        /// specified <paramref name="assembly"/> and adds them to the migration process.
        /// </summary>
        /// <param name="assembly">The assembly to scan.</param>
        protected virtual void FindMigrations(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes().Where(t => IsMigration(t)))
            {
                var attr = type.GetCustomAttributes(typeof(MigrationAttribute), false)[0] as MigrationAttribute;
                Migrations.Add(attr.Version, type);
            }
        }

        /// <summary>
        /// Creates a new instance of the migration of the specified <paramref name="version"/>.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <returns>The migration object.</returns>
        protected virtual IMigration GetMigration(long version)
        {
            return Activator.CreateInstance(Migrations[version]) as IMigration;
        }

        /// <summary>
        /// Applies the specified <paramref name="migration"/> within the
        /// specified <paramref name="context"/>.
        /// </summary>
        /// <param name="migration">The migration to apply.</param>
        /// <param name="direction">The migration direction.</param>
        /// <param name="context">The context.</param>
        protected virtual void ApplyMigration(IMigration migration, MigrationDirection direction, MigrationContext context)
        {
            //
            // Notify environment
            //
            var beforeArgs = new BeforeMigrationEventArgs(migration.GetVersion(), migration, direction);
            OnBeforeMigration(beforeArgs);
            if (beforeArgs.Cancel) throw new Exception("environment cancelled migration");

            //
            // Generate model
            //
            Database model = new Database(context);

            //
            // Send SQL statements on flush notification
            //
            model.FlushChanges += delegate(object sender, EventArgs e)
            {
                foreach (string sql in context.SqlProvider.GenerateSqlCommands(model))
                {
                    // Notify environment
                    var beforeSqlArgs = new BeforeSqlEventArgs(sql);
                    OnBeforeSql(beforeSqlArgs);
                    if (beforeSqlArgs.Cancel) throw new Exception("environment cancelled migration");

                    // Execute
                    SqlProcessor.ProcessMigrationStatement(context, sql);

                    // Notify environment
                    var afterSqlArgs = new AfterSqlEventArgs(sql, true);
                    OnAfterSql(afterSqlArgs);
                }
            };

            //
            // Run migration          
            //
            if (direction == MigrationDirection.Up)
                migration.Up(model);
            else
                migration.Down(model);

            //
            // Flush changes
            //
            model.Flush();

            //
            // Memorize migration
            //
            HistoryRepository.AddItem(context,
                new MigrationHistoryItem()
                {
                    Date = DateTime.Now,
                    Direction = direction,
                    Version = migration.GetVersion()
                }
            );

            //
            // Notify enviroment
            //
            var afterArgs = new AfterMigrationEventArgs(migration.GetVersion(), migration, direction, true);
            OnAfterMigration(afterArgs);
        }

        /// <summary>
        /// Gets the settings for the transaction that a migration is perfomed in.
        /// </summary>
        /// <returns>The transaction options.</returns>
        protected virtual TransactionOptions GetTransactionOptions()
        {
            return new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.Serializable,
                Timeout = TimeSpan.FromMinutes(15)
            };
        }

        #endregion

        #region Protected Properties

        /// <summary>
        /// Gets or sets the available migrations.
        /// </summary>
        /// <value>The migrations.</value>
        protected Dictionary<long, Type> Migrations
        {
            get;
            set;
        }

        #endregion

        #region Public Events

        /// <summary>
        /// Occurs before a migration is applied.
        /// </summary>
        public event EventHandler<BeforeMigrationEventArgs> BeforeMigration;

        /// <summary>
        /// Occurs after a migration is applied.
        /// </summary>
        public event EventHandler<AfterMigrationEventArgs> AfterMigration;

        /// <summary>
        /// Occurs before a SQL statement is executed.
        /// </summary>
        public event EventHandler<BeforeSqlEventArgs> BeforeSql;

        /// <summary>
        /// Occurs after a SQL statement was executed.
        /// </summary>
        public event EventHandler<AfterSqlEventArgs> AfterSql;

        /// <summary>
        /// Raises the <see cref="E:BeforeMigration"/> event.
        /// </summary>
        /// <param name="e">The <see cref="NMigrations.Core.BeforeMigrationEventArgs"/> instance containing the event data.</param>
        protected virtual void OnBeforeMigration(BeforeMigrationEventArgs e)
        {
            if (BeforeMigration != null)
            {
                BeforeMigration(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:AfterMigration"/> event.
        /// </summary>
        /// <param name="e">The <see cref="NMigrations.Core.AfterMigrationEventArgs"/> instance containing the event data.</param>
        protected virtual void OnAfterMigration(AfterMigrationEventArgs e)
        {
            if (AfterMigration != null)
            {
                AfterMigration(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:BeforeSql"/> event.
        /// </summary>
        /// <param name="e">The <see cref="NMigrations.Core.BeforeSqlEventArgs"/> instance containing the event data.</param>
        protected virtual void OnBeforeSql(BeforeSqlEventArgs e)
        {
            if (BeforeSql != null)
            {
                BeforeSql(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:AfterSql"/> event.
        /// </summary>
        /// <param name="e">The <see cref="NMigrations.Core.AfterSqlEventArgs"/> instance containing the event data.</param>
        protected virtual void OnAfterSql(AfterSqlEventArgs e)
        {
            if (AfterSql != null)
            {
                AfterSql(this, e);
            }
        }

        #endregion
    }
}