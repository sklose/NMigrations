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
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMigrations.Sql.SqlServer;

namespace NMigrations.Test
{
    [TestClass]
    public class SqlServerProviderTest : BaseTest
    {
        #region Private Properties

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>The target.</value>
        private SqlServerProvider_Accessor Target
        {
            get;
            set;
        }

        #endregion

        #region Initialize Test

        [TestInitialize]
        public void InitializeTarget()
        {
            Target = new SqlServerProvider_Accessor();
        }

        #endregion

        /// <summary>
        /// Checkes that the <see cref="Insert"/> method generates the
        /// appropriate INSERT SQL statement(s).
        /// </summary>
        [TestMethod]
        [DeploymentItem("NMigrations.dll")]
        public void InsertTest()
        {
            Database db = new Database(new MockMigrationContext(Target));
            db.AlterTable("MyTable").Insert(
                new 
                { 
                    MyColumn1 = new DateTime(1970, 2, 24),
                    MyColumn2 = "Test",
                    MyColumn3 = 23
                }
            );

            string[] sql = Target.Insert(db.MigrationSteps.ToArray()[1] as Insert).ToArray();
            Assert.AreEqual(1, sql.Length);
            Assert.AreEqual("INSERT INTO [MyTable] ([MyColumn1], [MyColumn2], [MyColumn3]) VALUES('1970-02-24', 'Test', 23);", sql[0]);
        }

        /// <summary>
        /// Checkes that the <see cref="Update"/> method generates the
        /// appropriate UPDATE SQL statement.
        /// </summary>
        [TestMethod]
        [DeploymentItem("NMigrations.dll")]
        public void UpdateTest()
        {
            Database db = new Database(new MockMigrationContext(Target));
            db.AlterTable("MyTable").Update(
                new
                {
                    Column1 = "NewValue",
                    Column2 = 3
                },
                new
                {
                    Column2 = 4,
                    Column3 = true
                }
            );

            string[] sql = Target.Update(db.MigrationSteps.ToArray()[1] as Update).ToArray();
            Assert.AreEqual(1, sql.Length);
            Assert.AreEqual("UPDATE [MyTable] SET [Column1] = 'NewValue', [Column2] = 3 WHERE [Column2] = 4 AND [Column3] = 1;", sql[0]);
        }

        /// <summary>
        /// Checks that the <see cref="GetQuerySeparator"/> method returns
        /// the query separator used by SQL Server.
        /// </summary>
        [TestMethod]
        public void GetQuerySeparatorTest()
        {
            Assert.AreEqual("GO", Target.GetQuerySeparator());
        }

        /// <summary>
        /// Checks that the <see cref="EscapeTableName"/> method escapes
        /// table names appropriately.
        /// </summary>
        [TestMethod]
        [DeploymentItem("NMigrations.dll")]
        public void EscapeTableNameTest()
        {
            Assert.AreEqual("[MyTable]", Target.EscapeTableName("MyTable"));
        }

        /// <summary>
        /// Checks that the <see cref="EscapeConstraintName"/> method escapes
        /// constraint names appropriately.
        /// </summary>
        [TestMethod]
        [DeploymentItem("NMigrations.dll")]
        public void EscapeConstraintNameTest()
        {
            Assert.AreEqual("[MyConstraint]", Target.EscapeConstraintName("MyConstraint"));
        }

        /// <summary>
        /// Checks that the <see cref="EscapeColumnName"/> method escapes
        /// column names appropriately.
        /// </summary>
        [TestMethod]
        [DeploymentItem("NMigrations.dll")]
        public void EscapeColumnNameTest()
        {
            Assert.AreEqual("[MyColumn]", Target.EscapeColumnName("MyColumn"));
        }

        /// <summary>
        ///A test for BuildDataType
        ///</summary>
        [TestMethod]
        [DeploymentItem("NMigrations.dll")]
        public void BuildDataTypeTest()
        {
            SqlTypes type = SqlTypes.Int;
            int? length = null, scale = null, precision = null;

            var DoCheck = new Action<string>(delegate(string expected)
            {
                Assert.AreEqual(expected, Target.BuildDataType(type, length, scale, precision));
            });

            type = SqlTypes.BigInt;
            length = scale = precision = null;
            DoCheck("BIGINT");

            type = SqlTypes.Boolean;
            length = scale = precision = null;
            DoCheck("BIT");

            type = SqlTypes.Char;
            length = scale = precision = null;
            DoCheck("CHAR");

            type = SqlTypes.Char;
            length = 3; scale = precision = null;
            DoCheck("CHAR(3)");

            type = SqlTypes.Currency;
            length = scale = precision = null;
            DoCheck("MONEY");

            type = SqlTypes.Currency;
            length = null; scale = 7; precision = 5;            
            DoCheck("MONEY(7, 5)");

            type = SqlTypes.Date;
            length = scale = precision = null;
            DoCheck("DATE");

            type = SqlTypes.DateTime;
            length = scale = precision = null;
            DoCheck("DATETIME");

            type = SqlTypes.Decimal;
            length = scale = precision = null;
            DoCheck("DECIMAL");

            type = SqlTypes.Decimal;
            length = scale = 10; precision = 2;
            DoCheck("DECIMAL(10, 2)");

            type = SqlTypes.Double;
            length = scale = precision = null;
            DoCheck("REAL");

            type = SqlTypes.Guid;
            length = scale = precision = null;
            DoCheck("UNIQUEIDENTIFIER");

            type = SqlTypes.Int;
            length = scale = precision = null;
            DoCheck("INT");

            type = SqlTypes.NChar;
            length = 7; scale = precision = null;
            DoCheck("NCHAR(7)");

            type = SqlTypes.NText;
            length = scale = precision = null;
            DoCheck("NTEXT");

            type = SqlTypes.NVarChar;
            length = scale = precision = null;
            DoCheck("NVARCHAR");

            type = SqlTypes.NVarCharMax;
            length = scale = precision = null;
            DoCheck("NVARCHAR(MAX)");

            type = SqlTypes.Single;
            length = scale = precision = null;
            DoCheck("FLOAT");

            type = SqlTypes.SmallInt;
            length = scale = precision = null;
            DoCheck("SMALLINT");

            type = SqlTypes.Text;
            length = scale = precision = null;
            DoCheck("TEXT");

            type = SqlTypes.Time;
            length = scale = precision = null;
            DoCheck("TIME");

            type = SqlTypes.TinyInt;
            length = scale = precision = null;
            DoCheck("TINYINT");

            type = SqlTypes.VarChar;
            length = scale = precision = null;
            DoCheck("VARCHAR");

            type = SqlTypes.VarCharMax;
            length = scale = precision = null;
            DoCheck("VARCHAR(MAX)");

            type = SqlTypes.Xml;
            length = scale = precision = null;
            DoCheck("XML");

            type = SqlTypes.TimeSpan;
            length = scale = precision = null;
            DoCheck("DATETIMEOFFSET");

            type = SqlTypes.TimeStamp;
            length = scale = precision = null;
            DoCheck("TIMESTAMP");
        }

        /// <summary>
        /// Checks that the <see cref="BuildAutoIncrement"/> methods
        /// generates the appropriate SQL fragment.
        /// </summary>
        [TestMethod]
        [DeploymentItem("NMigrations.dll")]
        public void BuildAutoIncrementTest()
        {
            string sql = Target.BuildAutoIncrement(null, null);
            Assert.AreEqual("IDENTITY(1, 1)", sql);

            sql = Target.BuildAutoIncrement(5, null);
            Assert.AreEqual("IDENTITY(5, 1)", sql);

            sql = Target.BuildAutoIncrement(5, 2);
            Assert.AreEqual("IDENTITY(5, 2)", sql);            
        }

        /// <summary>
        /// Checks that the <see cref="AddForeignKeyConstraint"/> method
        /// generates the appropriate SQL command.
        /// </summary>
        [TestMethod]
        public void AddForeignKeyConstraintTest()
        {
            Database db = new Database(new MockMigrationContext(Target));
            db.AlterTable("MyTable").AddForeignKeyConstraint("MyFK", "MyColumn", "MyRelatedTable", "MyRelatedColumn");
            var fk = db.MigrationSteps.FirstOrDefault(ms => ms is ForeignKeyConstraint) as ForeignKeyConstraint;

            string[] sql = Target.AddForeignKeyConstraint(fk).ToArray();
            Assert.AreEqual(1, sql.Length);
            Assert.AreEqual("ALTER TABLE [MyTable] ADD CONSTRAINT [MyFK] FOREIGN KEY ([MyColumn]) REFERENCES [MyRelatedTable] ([MyRelatedColumn]);", sql[0]);
        }

        /// <summary>
        /// Checks that the <see cref="AddPrimaryKeyConstraint"/> method
        /// generates the appropriate SQL command.
        /// </summary>
        [TestMethod]
        public void AddPrimaryKeyConstraintTest()
        {
            Database db = new Database(new MockMigrationContext(Target));
            db.AlterTable("MyTable").AddPrimaryKeyConstraint("MyPK", new string[] { "MyColumn1", "MyColumn2" });
            var pk = db.MigrationSteps.FirstOrDefault(ms => ms is PrimaryKeyConstraint) as PrimaryKeyConstraint;

            string[] sql = Target.AddPrimaryKeyConstraint(pk).ToArray();
            Assert.AreEqual(1, sql.Length);
            Assert.AreEqual("ALTER TABLE [MyTable] ADD CONSTRAINT [MyPK] PRIMARY KEY ([MyColumn1], [MyColumn2]);", sql[0]);
        }

        /// <summary>
        /// Checks that the <see cref="AddUniqueConstraint"/> method
        /// generates the appropriate SQL command.
        /// </summary>
        [TestMethod]
        public void AddUniqueConstraintTest()
        {
            Database db = new Database(new MockMigrationContext(Target));
            db.AlterTable("MyTable").AddUniqueConstraint("MyPK", new string[] { "MyColumn1" });
            var unique = db.MigrationSteps.FirstOrDefault(ms => ms is UniqueConstraint) as UniqueConstraint;

            string[] sql = Target.AddUniqueConstraint(unique).ToArray();
            Assert.AreEqual(1, sql.Length);
            Assert.AreEqual("ALTER TABLE [MyTable] ADD CONSTRAINT [MyPK] UNIQUE ([MyColumn1]);", sql[0]);
        }

        /// <summary>
        /// Checks that the appropriate CREATE TABLE statement is created for a
        /// simple database schema.
        /// </summary>
        [TestMethod]
        public void DatabaseModel1Test()
        {
            Database db = new Database(new MockMigrationContext(Target));
            var t = db.AddTable("Customers");
            t.AddColumn("CustomerID", SqlTypes.Int).PrimaryKey().AutoIncrement();
            t.AddColumn("Firstname", SqlTypes.NVarChar, 32).NotNull();
            t.AddColumn("Lastname", SqlTypes.NVarChar, 32).NotNull();

            string[] sql = Target.GenerateSqlCommands(db).ToArray();
            Assert.AreEqual(1, sql.Length);
            Assert.AreEqual("CREATE TABLE [Customers] (" + Environment.NewLine +
                "\t[CustomerID] INT NOT NULL IDENTITY(1, 1)," + Environment.NewLine +
                "\t[Firstname] NVARCHAR(32) NOT NULL," + Environment.NewLine +
                "\t[Lastname] NVARCHAR(32) NOT NULL," + Environment.NewLine +
                "\tCONSTRAINT [PK_Customers] PRIMARY KEY ([CustomerID])" + Environment.NewLine +
            ");", sql[0]);
        }

        /// <summary>
        /// Checks that the right SQL commands are created for adding
        /// and renaming a column.
        /// </summary>
        [TestMethod]
        public void DatabaseModel2Test()
        {
            Database db = new Database(new MockMigrationContext(Target));
            var t = db.AlterTable("Customers");            
            t.AddColumn("Firstname", SqlTypes.NVarChar, 32).NotNull();
            t.AlterColumn("Lastname").Rename("Surname");

            string[] sql = Target.GenerateSqlCommands(db).ToArray();
            Assert.AreEqual(2, sql.Length);
            Assert.AreEqual("ALTER TABLE [Customers] ADD [Firstname] NVARCHAR(32) NOT NULL;", sql[0]);
            Assert.AreEqual("EXEC sp_Rename '[Customers].[Lastname]', '[Surname]', 'COLUMN';", sql[1]);
        }

        /// <summary>
        /// Checks that the right SQL command is generated if the only
        /// operation is to add an index to a column.
        /// </summary>
        [TestMethod]
        public void DatabaseModel3Test()
        {
            Database db = new Database(new MockMigrationContext(Target));
            var t = db.AlterTable("Customers");
            t.AlterColumn("Firstname").Index("MyIndex");

            string[] sql = Target.GenerateSqlCommands(db).ToArray();
            Assert.AreEqual(1, sql.Length);
            Assert.AreEqual("CREATE INDEX [MyIndex] ON [Customers] ([Firstname]);", sql[0]);
        }

        /// <summary>
        /// Checks that the appropriate SQL commands are generated for dropping a
        /// column that has a default value.
        /// </summary>
        [TestMethod]
        public void DatabaseModel4Test()
        {
            Database db = new Database(new MockMigrationContext(Target));
            var t = db.AlterTable("Customers");
            t.AlterColumn("NewColumn").DropDefault();
            t.DropColumn("NewColumn");

            string[] sql = Target.GenerateSqlCommands(db).ToArray();
            Assert.AreEqual(2, sql.Length);
            Assert.AreEqual(
                "DECLARE @NewColumnDefaultName VARCHAR(MAX);" + Environment.NewLine +
                "SELECT @NewColumnDefaultName = o2.name " +
                "FROM syscolumns c " +
                "JOIN sysobjects o ON c.id = o.id " +
                "JOIN sysobjects o2 ON c.cdefault = o2.id " +
                "WHERE o.name = 'Customers' AND c.name = 'NewColumn';" + Environment.NewLine +
                "EXEC('ALTER TABLE [Customers] DROP CONSTRAINT ' + @NewColumnDefaultName);", 
                sql[0]
            );
            Assert.AreEqual("ALTER TABLE [Customers] DROP COLUMN [NewColumn];", sql[1]);
        }
    }
}