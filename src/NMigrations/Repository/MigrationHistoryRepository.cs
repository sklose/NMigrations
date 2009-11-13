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

using System.Collections.Generic;

namespace NMigrations.Repository
{
    /// <summary>
    /// Provides access to the migration history table in the database
    /// that stores information about previous migrations.
    /// </summary>
    public class MigrationHistoryRepository : IMigrationHistoryRepository
    {
        #region IMigrationHistoryRepository Members

        /// <summary>
        /// Retrieves information about all migrations that have
        /// been applied to the database so far.
        /// </summary>
        /// <param name="context">The migration context.</param>
        /// <returns>The migration history.</returns>
        public virtual MigrationHistoryItem[] RetrieveHistory(MigrationContext context)
        {
            bool error;
            return InternalRetrieveHistory(context, out error);
        }

        /// <summary>
        /// Stores a new migration history record in the database.
        /// </summary>
        /// <param name="context">The migration context.</param>
        /// <param name="item">The item.</param>
        public virtual void AddItem(MigrationContext context, MigrationHistoryItem item)
        {
            EnsureMigrationHistorySchemaExists(context);

            Database model = new Database(null);
            Table t = model.AlterTable(TableName);

            var row = new Dictionary<string, object>();
            row.Add(DateColumnName, item.Date);
            row.Add(VersionColumnName, item.Version);
            row.Add(DirectionColumnName, (int)item.Direction);
            t.Insert(row);

            foreach (string sql in context.SqlProvider.GenerateSqlCommands(model))
            {
                context.SqlProcessor.ProcessEngineStatement(context, sql);
            }

        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Retrieves information about all migrations that have
        /// been applied to the database so far.
        /// </summary>
        /// <param name="context">The migration context.</param>
        /// <param name="error">if set to <c>true</c> an error occured while querying the database.</param>
        /// <returns>The migration history.</returns>
        private MigrationHistoryItem[] InternalRetrieveHistory(MigrationContext context, out bool error)
        {
            error = false;
            var result = new List<MigrationHistoryItem>();
            try
            {
                using (var cmd = context.Connection.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT * FROM {0} ORDER BY {1}", TableName, DateColumnName);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new MigrationHistoryItem()
                            {
                                Date = reader.GetDateTime(reader.GetOrdinal(DateColumnName)),
                                Version = reader.GetInt64(reader.GetOrdinal(VersionColumnName)),
                                Direction = (MigrationDirection)(int)reader.GetByte(reader.GetOrdinal(DirectionColumnName))
                            });
                        }
                    }
                }
            }
            catch
            {
                /* ignore, schema doesn't exist yet */
                error = true;
            }

            return result.ToArray();
        }

        #endregion

        #region Protected Methods

        private bool schemaDone = false;

        /// <summary>
        /// Ensures that the schema for the migration history exists.
        /// </summary>
        /// <param name="context">The context.</param>
        protected virtual void EnsureMigrationHistorySchemaExists(MigrationContext context)
        {
            if (schemaDone) return;

            //
            // Check if schema already exists
            //
            bool error;
            InternalRetrieveHistory(context, out error);
            if (error)
            {
                //
                // Try to create schema ... if it already exits the
                // SQL commands will simply fail
                //
                Database model = new Database(null);
                Table t = model.AddTable(TableName);
                {
                    t.AddColumn(DateColumnName, SqlTypes.DateTime).NotNull();
                    t.AddColumn(VersionColumnName, SqlTypes.BigInt).NotNull();
                    t.AddColumn(DirectionColumnName, SqlTypes.TinyInt).NotNull();
                }

                foreach (string sql in context.SqlProvider.GenerateSqlCommands(model))
                {
                    try
                    {
                        context.SqlProcessor.ProcessMigrationStatement(context, sql);
                    }
                    catch
                    {
                        // The schema probaly already exists,
                        // just ignore the exception
                        break;
                    }
                }
            }

            schemaDone = true;
        }

        #endregion

        #region Protected Properties

        /// <summary>
        /// Gets the name of the migration history table.
        /// </summary>
        /// <value>The name of the table.</value>
        protected virtual string TableName
        {
            get { return "MigrationHistory"; }
        }

        /// <summary>
        /// Gets the name of the date column.
        /// </summary>
        /// <value>The name of the date column.</value>
        protected virtual string DateColumnName
        {
            get { return "Date"; }
        }

        /// <summary>
        /// Gets the name of the version column.
        /// </summary>
        /// <value>The name of the version column.</value>
        protected virtual string VersionColumnName
        {
            get { return "Version"; }
        }

        /// <summary>
        /// Gets the name of the direction column.
        /// </summary>
        /// <value>The name of the direction column.</value>
        protected virtual string DirectionColumnName
        {
            get { return "Direction"; }
        }

        #endregion
    }
}