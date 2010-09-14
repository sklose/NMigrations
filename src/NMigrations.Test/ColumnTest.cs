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

namespace NMigrations.Test
{
    [TestClass]
    public class ColumnTest : BaseTest
    {
        #region Private Properties

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>The target.</value>
        private Column Target
        {
            get;
            set;
        }

        #endregion

        #region Initialize Test

        [TestInitialize]
        public void InitializeTarget()
        {
            MigrationContext context = new MigrationContext()
            {
                SqlProvider = new Sql.SqlServer.SqlServerProvider()
            };
            Database db = new Database(context);
            Table t = db.AddTable("Table");
            Target = t.AddColumn("MyColumn", SqlTypes.Int);
        }

        #endregion

        /// <summary>
        /// Checks that the <see cref="Unique"/> methods creates a
        /// <see cref="UniqueConstraint"/> in the migration steps queue.
        /// </summary>
        [TestMethod]
        public void UniqueTest()
        {
            Target.Unique();
            Assert.AreEqual(2 /* table +  constraint */, Target.Table.Database.MigrationSteps.Count);
            var uq = Target.Table.Database.MigrationSteps.First(ms => ms is UniqueConstraint) as UniqueConstraint;
            Assert.AreEqual(1, uq.ColumnNames.Length);
            Assert.AreEqual(Target.Name, uq.ColumnNames[0]);
            Assert.AreEqual(Modifier.Add, uq.Modifier);
        }

        /// <summary>
        /// Checks that the <see cref="Type"/> method sets the appropriate
        /// data type for the <see cref="Column"/> object.
        /// </summary>
        [TestMethod]
        public void TypeTest()
        {
            Database db = new Database(new MigrationContext());
            var table = db.AddTable("MyTable");


            var c1 = table.AlterColumn("MyColumn1").Type(SqlTypes.Date);
            var c2 = table.AlterColumn("MyColumn2").Type(typeof(int));
            var c3 = table.AlterColumn("MyColumn3").Type(SqlTypes.VarChar, 32);
            var c4 = table.AlterColumn("MyColumn4").Type(typeof(decimal), 10, 2);
            var c5 = table.AlterColumn("MyColumn5").Type(typeof(string), 64);
            var c6 = table.AlterColumn("MyColumn6").Type(SqlTypes.Decimal, 18, 4);

            Assert.AreEqual("MyColumn1", c1.Name);
            Assert.AreEqual(SqlTypes.Date, c1.DataType);
            Assert.AreEqual(true, c1.IsNullable);
            Assert.AreEqual(null, c1.Length);
            Assert.AreEqual(null, c1.Precision);
            Assert.AreEqual(null, c1.Scale);

            Assert.AreEqual("MyColumn2", c2.Name);
            Assert.AreEqual(SqlTypes.Int, c2.DataType);
            Assert.AreEqual(true, c2.IsNullable);
            Assert.AreEqual(null, c2.Length);
            Assert.AreEqual(null, c2.Precision);
            Assert.AreEqual(null, c2.Scale);

            Assert.AreEqual("MyColumn3", c3.Name);
            Assert.AreEqual(SqlTypes.VarChar, c3.DataType);
            Assert.AreEqual(true, c3.IsNullable);
            Assert.AreEqual(32, c3.Length);
            Assert.AreEqual(null, c3.Precision);
            Assert.AreEqual(null, c3.Scale);

            Assert.AreEqual("MyColumn4", c4.Name);
            Assert.AreEqual(SqlTypes.Decimal, c4.DataType);
            Assert.AreEqual(true, c4.IsNullable);
            Assert.AreEqual(null, c4.Length);
            Assert.AreEqual(2, c4.Precision);
            Assert.AreEqual(10, c4.Scale);

            Assert.AreEqual("MyColumn5", c5.Name);
            Assert.AreEqual(SqlTypes.NVarChar, c5.DataType);
            Assert.AreEqual(true, c5.IsNullable);
            Assert.AreEqual(64, c5.Length);
            Assert.AreEqual(null, c5.Precision);
            Assert.AreEqual(null, c5.Scale);

            Assert.AreEqual("MyColumn6", c6.Name);
            Assert.AreEqual(SqlTypes.Decimal, c6.DataType);
            Assert.AreEqual(true, c6.IsNullable);
            Assert.AreEqual(null, c6.Length);
            Assert.AreEqual(4, c6.Precision);
            Assert.AreEqual(18, c6.Scale);
        }

        /// <summary>
        /// Checks that the <see cref="Rename"/> method stores the
        /// new name for the column.
        /// </summary>
        [TestMethod]
        public void RenameTest()
        {
            Assert.IsNull(Target.NewName);
            Target.Rename("MyNewColumnName");
            Assert.AreEqual("MyNewColumnName", Target.NewName);
        }

        /// <summary>
        /// Checks that the <see cref="References"/> method creates a
        /// <see cref="ForeignKeyConstraint"/> in the migration steps queue.
        /// </summary>
        [TestMethod]
        public void ReferencesTest()
        {
            Target.References("MyRelatedTable", "MyRelatedColumn");
            Assert.AreEqual(2 /* table + constraint */, Target.Table.Database.MigrationSteps.Count);
            var r = Target.Table.Database.MigrationSteps.ToArray()[1] as ForeignKeyConstraint;

            Assert.AreEqual(Target.Table.Name, r.Table.Name);
            Assert.AreEqual("MyRelatedTable", r.RelatedTableName);
            Assert.AreEqual(1, r.ColumnNames.Length);
            Assert.AreEqual(Target.Name, r.ColumnNames[0]);
            Assert.AreEqual(1, r.RelatedColumnNames.Length);
            Assert.AreEqual("MyRelatedColumn", r.RelatedColumnNames[0]);
            Assert.AreEqual(Modifier.Add, r.Modifier);
        }

        /// <summary>
        /// Checks that the <see cref="PrimaryKey"/> method creates a
        /// <see cref="PrimaryKeyConstraint"/> in the migration steps queue.
        /// </summary>
        [TestMethod]
        public void PrimaryKeyTest()
        {
            Target.PrimaryKey();
            Assert.AreEqual(2 /* table + constraint */, Target.Table.Database.MigrationSteps.Count);
            var pk = Target.Table.Database.MigrationSteps.ToArray()[1] as PrimaryKeyConstraint;

            Assert.AreEqual(Target.Table.Name, pk.Table.Name);
            Assert.AreEqual(1, pk.ColumnNames.Length);
            Assert.AreEqual(Target.Name, pk.ColumnNames[0]);
            Assert.AreEqual(Modifier.Add, pk.Modifier);
        }

        /// <summary>
        /// Checks that the <see cref="NotNull"/> method switches the nullable flag.
        /// </summary>
        [TestMethod]
        public void NotNullTest()
        {
            Target.NotNull();
            Assert.IsFalse(Target.IsNullable);
        }

        /// <summary>
        /// Checks that the <see cref="PrimaryKey"/> method creates an
        /// <see cref="Index"/> in the migration steps queue.
        /// </summary>
        [TestMethod]
        public void IndexTest()
        {
            Target.Index();
            Assert.AreEqual(2 /* table + index */, Target.Table.Database.MigrationSteps.Count);
            var index = Target.Table.Database.MigrationSteps.ToArray()[1] as Index;

            Assert.AreEqual(Target.Table.Name, index.Table.Name);
            Assert.AreEqual(1, index.ColumnNames.Length);
            Assert.AreEqual(Target.Name, index.ColumnNames[0]);
            Assert.AreEqual(Modifier.Add, index.Modifier);
        }

        /// <summary>
        /// Checks that the <see cref="Default"/> method creates a <see cref="DefaultConstraint"/>
        /// in the migration steps queue.
        /// </summary>
        [TestMethod]
        public void DefaultTest()
        {
            Target.Default("MyDefaultValue");
            Assert.AreEqual(2 /* table + default */, Target.Table.Database.MigrationSteps.Count);
            var defaultConstraint = Target.Table.Database.MigrationSteps.ToArray()[1] as DefaultConstraint;

            Assert.AreEqual(Target.Table.Name, defaultConstraint.Table.Name);
            Assert.AreEqual(Target.Name, defaultConstraint.ColumnName);
            Assert.AreEqual(Modifier.Add, defaultConstraint.Modifier);
            Assert.AreEqual("MyDefaultValue", defaultConstraint.Value);
        }

        /// <summary>
        /// Checks that the <see cref="DropDefault"/> method creates a <see cref="DefaultConstraint"/>
        /// in the migration steps queue.
        /// </summary>
        [TestMethod]
        public void DropDefaultTest()
        {
            Target.DropDefault();
            Assert.AreEqual(2 /* table + default */, Target.Table.Database.MigrationSteps.Count);
            var defaultConstraint = Target.Table.Database.MigrationSteps.ToArray()[0] as DefaultConstraint;

            Assert.AreEqual(Target.Table.Name, defaultConstraint.Table.Name);
            Assert.AreEqual(Target.Name, defaultConstraint.ColumnName);
            Assert.AreEqual(Modifier.Drop, defaultConstraint.Modifier);
        }

        /// <summary>
        /// Checks that the <see cref="BelongsToPrimaryKey"/> method returns
        /// the appropriate value.
        /// </summary>
        [TestMethod]
        public void BelongsToPrimaryKeyTest()
        {
            Assert.IsFalse(Target.BelongsToPrimaryKey());
            Target.PrimaryKey();
            Assert.IsTrue(Target.BelongsToPrimaryKey());
        }

        /// <summary>
        /// Checks that the <see cref="BelongsToForeignKey"/> method returns
        /// the appropriate value.
        /// </summary>
        [TestMethod]
        public void BelongsToForeignKeyTest()
        {
            Assert.IsFalse(Target.BelongsToForeignKey());
            Target.References("MyReferencedTable", "MyReferencedColumn");
            Assert.IsTrue(Target.BelongsToForeignKey());
        }

        /// <summary>
        /// Checks that the <see cref="GetForeignKey"/> method returns
        /// the appropriate value.
        /// </summary>
        [TestMethod]
        public void GetForeignKeyTest()
        {
            Assert.IsNull(Target.GetForeignKeyConstraint());

            Assert.IsFalse(Target.BelongsToForeignKey());
            Target.References("MyReferencedTable", "MyReferencedColumn");
            Assert.IsNotNull(Target.GetForeignKeyConstraint());
            Assert.AreEqual(Target.Table, Target.GetForeignKeyConstraint().Table);
            Assert.AreEqual(1, Target.GetForeignKeyConstraint().ColumnNames.Length);
            Assert.AreEqual(Target.Name, Target.GetForeignKeyConstraint().ColumnNames[0]);
            Assert.AreEqual("MyReferencedTable", Target.GetForeignKeyConstraint().RelatedTableName);
            Assert.AreEqual(1, Target.GetForeignKeyConstraint().RelatedColumnNames.Length);
            Assert.AreEqual("MyReferencedColumn", Target.GetForeignKeyConstraint().RelatedColumnNames[0]);

        }

        /// <summary>
        /// Checks that the <see cref="AutoIncrement"/> method sets the
        /// <see cref="IsAutoIncrement"/> flag.
        /// </summary>
        [TestMethod]
        public void AutoIncrementWithoutParametersTest()
        {
            Target.AutoIncrement();
            Assert.IsTrue(Target.IsAutoIncrement);
        }

        /// <summary>
        /// Checks that the <see cref="AutoIncrement"/> method sets the
        /// <see cref="IsAutoIncrement"/> flag and stores the seed/step
        /// </summary>
        [TestMethod]
        public void AutoIncrementWithParametersTest()
        {
            Target.AutoIncrement(5, 2);
            Assert.IsTrue(Target.IsAutoIncrement);
            Assert.AreEqual(5, Target.AutoIncrementSeed);
            Assert.AreEqual(2, Target.AutoIncrementStep);
        }

        /// <summary>
        /// Checks that the constructor initializes all private variables
        /// to their default values.
        /// </summary>
        [TestMethod]
        public void ColumnConstructorTest()
        {
            Assert.IsTrue(Target.IsNullable);
            Assert.IsFalse(Target.IsAutoIncrement);
            Assert.IsNull(Target.AutoIncrementSeed);
            Assert.IsNull(Target.AutoIncrementStep);
            Assert.IsNull(Target.Length);
            Assert.IsNull(Target.NewName);
            Assert.IsNull(Target.Precision);
            Assert.IsNull(Target.Scale);
        }
    }
}