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
        [TestMethod()]
        [DeploymentItem("NMigrations.dll")]
        public void InsertTest()
        {
            Database db = new Database(new MigrationContext());
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
        /// Checks that the <see cref="GetQuerySeparator"/> method returns
        /// the query separator used by SQL Server.
        /// </summary>
        [TestMethod()]
        public void GetQuerySeparatorTest()
        {
            Assert.AreEqual("GO", Target.GetQuerySeparator());
        }

        /// <summary>
        /// Checks that the <see cref="EscapeTableName"/> method escapes
        /// table names appropriately.
        /// </summary>
        [TestMethod()]
        [DeploymentItem("NMigrations.dll")]
        public void EscapeTableNameTest()
        {
            Assert.AreEqual("[MyTable]", Target.EscapeTableName("MyTable"));
        }

        /// <summary>
        /// Checks that the <see cref="EscapeConstraintName"/> method escapes
        /// constraint names appropriately.
        /// </summary>
        [TestMethod()]
        [DeploymentItem("NMigrations.dll")]
        public void EscapeConstraintNameTest()
        {
            Assert.AreEqual("[MyConstraint]", Target.EscapeConstraintName("MyConstraint"));
        }

        /// <summary>
        /// Checks that the <see cref="EscapeColumnName"/> method escapes
        /// column names appropriately.
        /// </summary>
        [TestMethod()]
        [DeploymentItem("NMigrations.dll")]
        public void EscapeColumnNameTest()
        {
            Assert.AreEqual("[MyColumn]", Target.EscapeColumnName("MyColumn"));
        }

        /// <summary>
        ///A test for BuildDataType
        ///</summary>
        [TestMethod()]
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
        [TestMethod()]
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
    }
}
