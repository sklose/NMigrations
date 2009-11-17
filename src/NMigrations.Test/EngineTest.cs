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

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NMigrations.Core;
using NMigrations.Repository;
using NMigrations.Sql;

namespace NMigrations.Test
{
    [TestClass]
    public class EngineTest : BaseTest
    {
        #region Private Types

        private const int ValidMigrationVersion = 243;

        [Migration(ValidMigrationVersion)]
        private class ValidMigration : IMigration
        {
            #region IMigration Members

            public void Up(Database db)
            {
                
            }

            public void Down(Database db)
            {
                
            }

            #endregion
        }

        // Attribute is missing
        private class InvalidMigration : IMigration
        {
            #region IMigration Members

            public void Up(Database db)
            {
                
            }

            public void Down(Database db)
            {
                
            }

            #endregion
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>The target.</value>
        private Engine_Accessor Target
        {
            get;
            set;
        }

        #endregion

        #region Initialize Test

        [TestInitialize]
        public void InitializeTarget()
        {
            Target = new Engine_Accessor();
            Target.ProviderInvariantName = "System.Data.SqlClient";
        }

        #endregion

        /// <summary>
        /// Checks that the <see cref="SetSqlProviderFactory"/> 
        /// sets the <see cref="SqlServerProvider"/> appropriately.
        /// </summary>
        [TestMethod]
        public void SetSqlProviderFactoryTest()
        {
            SqlProviderFactory pf = new SqlProviderFactory();
            Target.SetSqlProviderFactory(pf);
            Assert.AreEqual(pf, Target.SqlProviderFactory);
        }

        /// <summary>
        /// Checks that the <see cref="SetSqlProcessor"/> 
        /// sets the <see cref="ISqlProcessor"/> appropriately.
        /// </summary>
        [TestMethod]
        public void SetSqlProcessorTest()
        {
            ISqlProcessor processor = new DatabaseSqlProcessor();
            Target.SetSqlProcessor(processor);
            Assert.AreEqual(processor, Target.SqlProcessor);
        }

        /// <summary>
        /// Checks that the <see cref="SetConnectionString"/> 
        /// sets the <see cref="ConnectionString"/> and the 
        /// <see cref="ProvidrInvariantName" /> appropriately.
        /// </summary>
        [TestMethod]
        public void SetConnectionStringTest()
        {
            string provider = "System.Data.MyProvider";
            string cs = "DataSource=.; IntegratedSecurity=True";
            Target.SetConnectionString(provider, cs);
            Assert.AreEqual(provider, Target.ProviderInvariantName);
            Assert.AreEqual(cs, Target.ConnectionString);
        }

        /// <summary>
        ///A test for RegisterProvider
        ///</summary>
        [TestMethod]
        public void RegisterProviderTest()
        {
            Assert.Inconclusive("TODO");
        }

        /// <summary>
        /// Checks that the <see cref="OpenConnection"/> methods returns
        /// an open database connection with the configured <see cref="ConnectionString"/>.
        /// </summary>
        [TestMethod]
        [DeploymentItem("NMigrations.dll")]
        public void OpenConnectionTest()
        {
            Assert.Inconclusive("TODO");
        }

        /// <summary>
        /// Checks that the <see cref="OnBeforeSql"/> method raises the
        /// <see cref="BeforeSql"/> event.
        /// </summary>
        [TestMethod]
        [DeploymentItem("NMigrations.dll")]
        public void OnBeforeSqlTest()
        {
            bool fired = false;
            ((Engine)Target.Target).BeforeSql += delegate(object sender, BeforeSqlEventArgs e)
            {
                fired = true;
            };
            Target.OnBeforeSql(new BeforeSqlEventArgs("SQL STATEMENT"));
            Assert.IsTrue(fired);
        }

        /// <summary>
        /// Checks that the <see cref="OnBeforeMigration"/> method raises the
        /// <see cref="BeforeMigration"/> event.
        /// </summary>
        [TestMethod]
        [DeploymentItem("NMigrations.dll")]
        public void OnBeforeMigrationTest()
        {
            bool fired = false;
            ((Engine)Target.Target).BeforeMigration += delegate(object sender, BeforeMigrationEventArgs e)
            {
                fired = true;
            };
            Target.OnBeforeMigration(new BeforeMigrationEventArgs(1, null, MigrationDirection.Up));
            Assert.IsTrue(fired);
        }

        /// <summary>
        /// Checks that the <see cref="OnAfterSql"/> method raises the
        /// <see cref="AfterSql"/> event.
        /// </summary>
        [TestMethod]
        [DeploymentItem("NMigrations.dll")]
        public void OnAfterSqlTest()
        {
            bool fired = false;
            ((Engine)Target.Target).AfterSql += delegate(object sender, AfterSqlEventArgs e)
            {
                fired = true;
            };
            Target.OnAfterSql(new AfterSqlEventArgs("SQL STATEMENT", true));
            Assert.IsTrue(fired);
        }

        /// <summary>
        /// Checks that the <see cref="OnAfterMigration"/> method raises the
        /// <see cref="AfterMigration"/> event.
        /// </summary>
        [TestMethod]
        [DeploymentItem("NMigrations.dll")]
        public void OnAfterMigrationTest()
        {
            bool fired = false;
            ((Engine)Target.Target).AfterMigration += delegate(object sender, AfterMigrationEventArgs e)
            {
                fired = true;
            };
            Target.OnAfterMigration(new AfterMigrationEventArgs(1, null, MigrationDirection.Up, true));
            Assert.IsTrue(fired);
        }

        /// <summary>
        ///A test for Migrate
        ///</summary>
        [TestMethod]
        public void MigrateTest()
        {
            Assert.Inconclusive("TODO");
        }

        /// <summary>
        /// Checks that the <see cref="IsMigration"/> method returns the
        /// appropriate value.
        /// </summary>
        [TestMethod]
        [DeploymentItem("NMigrations.dll")]
        public void IsMigrationTest()
        {
            Assert.IsFalse(Target.IsMigration(GetType()));
            Assert.IsFalse(Target.IsMigration(typeof(InvalidMigration)));
            Assert.IsTrue(Target.IsMigration(typeof(ValidMigration)));
        }

        /// <summary>
        /// Checks that the <see cref="GetTransactionOptions"/> method
        /// returns <see cref="System.Data.IsolationLevel.Serializable"/>
        /// as the default isolation level.
        /// </summary>
        [TestMethod]
        [DeploymentItem("NMigrations.dll")]
        public void GetTransactionOptionsTest()
        {
            var options = Target.GetTransactionOptions();
            Assert.AreEqual(System.Transactions.IsolationLevel.Serializable, options.IsolationLevel);
        }

        /// <summary>
        /// A test for GetMigration
        /// </summary>
        [TestMethod]
        [DeploymentItem("NMigrations.dll")]
        public void GetMigrationTest()
        {
            Assert.Inconclusive("TODO");
        }

        /// <summary>
        /// A test for FindMigrations
        /// </summary>
        [TestMethod]
        [DeploymentItem("NMigrations.dll")]
        public void FindMigrationsTest()
        {
            Assert.Inconclusive("TODO");
        }

        /// <summary>
        /// A test for CreateContext
        /// </summary>
        [TestMethod]
        [DeploymentItem("NMigrations.dll")]
        public void CreateContextTest()
        {
            Assert.Inconclusive("TODO");
        }

        /// <summary>
        /// A test for BuildMigrationPath
        /// </summary>
        [TestMethod]
        [DeploymentItem("NMigrations.dll")]
        public void BuildMigrationPathTest()
        {
            Assert.Inconclusive("TODO");
        }

        /// <summary>
        /// A test for ApplyMigration
        /// </summary>
        [TestMethod]
        [DeploymentItem("NMigrations.dll")]
        public void ApplyMigrationTest()
        {
            Assert.Inconclusive("TODO");
        }

        /// <summary>
        /// A test for AddAssembly
        /// </summary>
        [TestMethod]
        public void AddAssemblyTest()
        {
            Assert.Inconclusive("TODO");
        }

        /// <summary>
        /// Checks that the constructor initializes all properties appropriately.
        /// </summary>
        [TestMethod]
        public void EngineConstructorTest()
        {
            Engine_Accessor target = new Engine_Accessor();
            Assert.IsNull(target.ConnectionString);
            Assert.IsInstanceOfType(target.HistoryRepository, typeof(MigrationHistoryRepository));
            Assert.IsNull(target.ProviderInvariantName);
            Assert.IsInstanceOfType(target.SqlProcessor, typeof(DatabaseSqlProcessor));
            Assert.IsInstanceOfType(target.SqlProviderFactory, typeof(SqlProviderFactory));
            Assert.AreEqual(0, target.Migrations.Count);
        }
    }
}
