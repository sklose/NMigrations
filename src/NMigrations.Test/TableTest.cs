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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NMigrations.Test
{
    [TestClass]
    public class TableTest : BaseTest
    {
        #region Private Properties

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>The target.</value>
        private Table Target
        {
            get;
            set;
        }

        #endregion

        #region Initialize Test

        [TestInitialize]
        public void InitializeTarget()
        {
            MigrationContext context = new MigrationContext();
            Database db = new Database(context);
            Target = db.AddTable("Table");
        }

        #endregion

        /// <summary>
        /// Checks that all overloads of the <see cref="Insert"/> method create
        /// appropriate migration steps in the parent <see cref="Database"/> object.
        /// </summary>
        [TestMethod]
        public void InsertTest()
        {
            var row1 = new { Field1 = "row1", Field2 = 1 };
            var row2 = new Dictionary<string, object>();
            row2.Add("Field1", "row2");
            row2.Add("Field2", 2);

            Target.Insert(row1);
            Target.Insert(row2);

            Assert.AreEqual(3 /* AddTable + 2x Insert */, Target.Database.MigrationSteps.Count, "invalid number of migration steps");
            var steps = Target.Database.MigrationSteps.ToArray();

            for (int i = 1; i <= 2; i++)
            {
                Element e = steps[i];
                Assert.IsInstanceOfType(e, typeof(Insert));
                Insert ins = e as Insert;

                Assert.AreEqual(2, ins.Row.Keys.Count);
                Assert.IsTrue(ins.Row.ContainsKey("Field1"));
                Assert.IsTrue(ins.Row.ContainsKey("Field2"));

                Assert.AreEqual("row" + i.ToString(), ins.Row["Field1"]);
                Assert.AreEqual(i, ins.Row["Field2"]);
            }
        }

        /// <summary>
        /// Checks that the <see cref="HasPrimaryKey"/> method returns <c>true</c>
        /// if a primary key constraint exists or <c>false</c> otherwise.
        /// </summary>
        [TestMethod]
        public void HasPrimaryKeyTest()
        {
            Assert.IsFalse(Target.HasPrimaryKey());
            Target.AddPrimaryKeyConstraint(null, "MyColumn");
            Assert.IsTrue(Target.HasPrimaryKey());
        }

        /// <summary>
        /// Checks that the <see cref="HasCompoundPrimaryKey"/> method returns
        /// <c>true</c> if a a compound primary key exists.
        /// </summary>
        [TestMethod]
        public void HasCompoundPrimaryKeyTest_WithCompountPrimaryKey()
        {
            Assert.IsFalse(Target.HasCompoundPrimaryKey());
            Target.AddPrimaryKeyConstraint(null, new string[] { "Column1", "Column2" });
            Assert.IsTrue(Target.HasCompoundPrimaryKey());
        }

        /// <summary>
        /// Checks that the <see cref="HasCompoundPrimaryKey"/> method returns
        /// <c>false</c> if a a simple primary key exists.
        /// </summary>
        [TestMethod]
        public void HasCompoundPrimaryKeyTest_WithSimplePrimaryKey()
        {
            Assert.IsFalse(Target.HasCompoundPrimaryKey());
            Target.AddPrimaryKeyConstraint(null, "Column1");
            Assert.IsFalse(Target.HasCompoundPrimaryKey());
        }

        /// <summary>
        /// Checks that the <see cref="DropIndex"/> method creates an appropriate
        /// migration step in the parent <see cref="Database"/> object.
        /// </summary>
        [TestMethod]
        public void DropIndexTest()
        {
            Target.DropIndex("MyIndex");
            Assert.AreEqual(2 /* table + drop */, Target.Database.MigrationSteps.Count, "invalid number of migration steps");
            
            var e = Target.Database.MigrationSteps.ToArray()[1];
            Assert.IsInstanceOfType(e, typeof(Index));

            var i = e as Index;
            Assert.AreEqual(Modifier.Drop, i.Modifier);
            Assert.AreEqual("MyIndex", i.Name);
        }

        /// <summary>
        /// Checks that the <see cref="DropConstraint"/> method creates an appropriate
        /// migration step in the parent <see cref="Database"/> object.
        /// </summary>
        [TestMethod]
        public void DropConstraintTest()
        {
            Target.DropConstraint("MyConstraint");
            Assert.AreEqual(2 /* table + drop */, Target.Database.MigrationSteps.Count, "invalid number of migration steps");

            var e = Target.Database.MigrationSteps.ToArray()[1];
            Assert.IsInstanceOfType(e, typeof(Constraint));

            var i = e as Constraint;
            Assert.AreEqual(Modifier.Drop, i.Modifier);
            Assert.AreEqual("MyConstraint", i.Name);
        }

        /// <summary>
        /// Checks that the <see cref="DropColumn"/> method adds the approriate
        /// column item the the <see cref="Table.Columns"/> list.
        /// </summary>
        [TestMethod]
        public void DropColumnTest()
        {
            Target.DropColumn("MyColumn");
            Assert.AreEqual(1, Target.Columns.Count, "invalid number of columns");
            Assert.AreEqual(Modifier.Drop, Target.Columns[0].Modifier);
            Assert.AreEqual("MyColumn", Target.Columns[0].Name);
        }

        /// <summary>
        /// Checks that the <see cref="AlterColumn"/> method adds the approriate
        /// column item the the <see cref="Table.Columns"/> list.
        /// </summary>
        [TestMethod]
        public void AlterColumnTest()
        {
            Column c = Target.AlterColumn("MyColumn");
            Assert.AreEqual(1, Target.Columns.Count, "invalid number of columns");
            Assert.AreEqual(c, Target.Columns[0]);
            Assert.AreEqual(Modifier.Alter, Target.Columns[0].Modifier);
            Assert.AreEqual("MyColumn", Target.Columns[0].Name);
        }

        /// <summary>
        /// Checks that the <see cref="AddPrimaryKeyConstraint"/> method adds an 
        /// <see cref="UniqueConstraint"/> to the <see cref="Database.MigrationSteps"/> queue.
        /// </summary>
        [TestMethod]
        public void AddUniqueConstraintTest()
        {
            UniqueConstraint uc = Target.AddUniqueConstraint("MyConstraint", "MyColumn");
            Assert.AreEqual(2 /* table + constraint */, Target.Database.MigrationSteps.Count, "invalid number of migration steps");
            
            Assert.AreEqual(uc, Target.Database.MigrationSteps.ToArray()[1]);
            Assert.AreEqual("MyConstraint", uc.Name);
            Assert.AreEqual(1, uc.ColumnNames.Length);            
            Assert.AreEqual("MyColumn", uc.ColumnNames[0]);
            Assert.AreEqual(Modifier.Add, uc.Modifier);
        }

        /// <summary>
        /// Checks that the <see cref="AddPrimaryKeyConstraint"/> method adds an 
        /// <see cref="PrimaryKeyConstraint"/> to the <see cref="Database.MigrationSteps"/> queue.
        /// </summary>
        [TestMethod]
        public void AddPrimaryKeyConstraintTest()
        {
            PrimaryKeyConstraint pk = Target.AddPrimaryKeyConstraint("MyConstraint", "MyColumn");
            Assert.AreEqual(2 /* table + constraint */, Target.Database.MigrationSteps.Count, "invalid number of migration steps");

            Assert.AreEqual(pk, Target.Database.MigrationSteps.ToArray()[1]);
            Assert.AreEqual("MyConstraint", pk.Name);
            Assert.AreEqual(1, pk.ColumnNames.Length);
            Assert.AreEqual("MyColumn", pk.ColumnNames[0]);
            Assert.AreEqual(Modifier.Add, pk.Modifier);
        }

        /// <summary>
        /// Checks that the <see cref="AddPrimaryKeyConstraint"/> method adds an 
        /// <see cref="Index"/> to the <see cref="Database.MigrationSteps"/> queue.
        /// </summary>
        [TestMethod]
        public void AddIndexTest()
        {
            Index idx = Target.AddIndex("MyIndex", "MyColumn");
            Assert.AreEqual(2 /* table + constraint */, Target.Database.MigrationSteps.Count, "invalid number of migration steps");

            Assert.AreEqual(idx, Target.Database.MigrationSteps.ToArray()[1]);
            Assert.AreEqual("MyIndex", idx.Name);
            Assert.AreEqual(1, idx.ColumnNames.Length);
            Assert.AreEqual("MyColumn", idx.ColumnNames[0]);
            Assert.AreEqual(Modifier.Add, idx.Modifier);
        }

        /// <summary>
        /// Checks that the <see cref="AddForeignKeyConstraint"/> method adds an 
        /// <see cref="ForeignKeyConstraint"/> to the <see cref="Database.MigrationSteps"/> queue.
        /// </summary>
        [TestMethod]
        public void AddForeignKeyConstraintTest1()
        {
            ForeignKeyConstraint fk1 = Target.AddForeignKeyConstraint("MyConstraint", "MyColumn", "MyRelatedTable", "MyRelatedColumn");
            ForeignKeyConstraint fk2 = Target.AddForeignKeyConstraint("MyConstraint", new string[] { "MyColumn1", "MyColumn2" }, "MyRelatedTable", new string[] { "MyRelatedColumn1", "MyRelatedColumn2" });
            Assert.AreEqual(3 /* table + constraint */, Target.Database.MigrationSteps.Count, "invalid number of migration steps");

            Assert.AreEqual(fk1, Target.Database.MigrationSteps.ToArray()[1]);
            Assert.AreEqual("MyConstraint", fk1.Name);
            Assert.AreEqual(1, fk1.ColumnNames.Length);
            Assert.AreEqual("MyColumn", fk1.ColumnNames[0]);
            Assert.AreEqual(Modifier.Add, fk1.Modifier);
            Assert.AreEqual("MyRelatedTable", fk1.RelatedTableName);
            Assert.AreEqual(1, fk1.RelatedColumnNames.Length);
            Assert.AreEqual("MyRelatedColumn", fk1.RelatedColumnNames[0]);

            Assert.AreEqual(fk2, Target.Database.MigrationSteps.ToArray()[2]);
            Assert.AreEqual("MyConstraint", fk2.Name);
            Assert.AreEqual(2, fk2.ColumnNames.Length);
            Assert.AreEqual("MyColumn1", fk2.ColumnNames[0]);
            Assert.AreEqual("MyColumn2", fk2.ColumnNames[1]);
            Assert.AreEqual(Modifier.Add, fk1.Modifier);
            Assert.AreEqual("MyRelatedTable", fk2.RelatedTableName);
            Assert.AreEqual(2, fk2.RelatedColumnNames.Length);
            Assert.AreEqual("MyRelatedColumn1", fk2.RelatedColumnNames[0]);
            Assert.AreEqual("MyRelatedColumn2", fk2.RelatedColumnNames[1]);

        }

        /// <summary>
        /// Checks that the <see cref="AddColumn"/> method adds an 
        /// <see cref="Column"/> to the <see cref="Database.Columns"/> list.
        /// </summary>
        [TestMethod]
        public void AddColumnTest5()
        {
            var c1 = Target.AddColumn("MyColumn1", SqlTypes.Date);
            var c2 = Target.AddColumn("MyColumn2", typeof(int));
            var c3 = Target.AddColumn("MyColumn3", SqlTypes.VarChar, 32);
            var c4 = Target.AddColumn("MyColumn4", typeof(decimal), 10, 2);
            var c5 = Target.AddColumn("MyColumn5", typeof(string), 64);
            var c6 = Target.AddColumn("MyColumn6", SqlTypes.Decimal, 18, 4);
            Assert.AreEqual(6, Target.Columns.Count);

            Assert.AreEqual(c1, Target.Columns[0]);
            Assert.AreEqual("MyColumn1", c1.Name);
            Assert.AreEqual(SqlTypes.Date, c1.DataType);
            Assert.AreEqual(null, c1.AutoIncrementSeed);
            Assert.AreEqual(null, c1.AutoIncrementStep);
            Assert.AreEqual(null, c1.DefaultValue);
            Assert.AreEqual(false, c1.IsAutoIncrement);
            Assert.AreEqual(true, c1.IsNullable);
            Assert.AreEqual(null, c1.Length);
            Assert.AreEqual(null, c1.NewName);
            Assert.AreEqual(null, c1.Precision);
            Assert.AreEqual(null, c1.Scale);
            Assert.AreEqual(Modifier.Add, c1.Modifier);

            Assert.AreEqual(c2, Target.Columns[1]);
            Assert.AreEqual("MyColumn2", c2.Name);
            Assert.AreEqual(SqlTypes.Int, c2.DataType);
            Assert.AreEqual(null, c2.AutoIncrementSeed);
            Assert.AreEqual(null, c2.AutoIncrementStep);
            Assert.AreEqual(null, c2.DefaultValue);
            Assert.AreEqual(false, c2.IsAutoIncrement);
            Assert.AreEqual(true, c2.IsNullable);
            Assert.AreEqual(null, c2.Length);
            Assert.AreEqual(null, c2.NewName);
            Assert.AreEqual(null, c2.Precision);
            Assert.AreEqual(null, c2.Scale);
            Assert.AreEqual(Modifier.Add, c2.Modifier);

            Assert.AreEqual(c3, Target.Columns[2]);
            Assert.AreEqual("MyColumn3", c3.Name);
            Assert.AreEqual(SqlTypes.VarChar, c3.DataType);
            Assert.AreEqual(null, c3.AutoIncrementSeed);
            Assert.AreEqual(null, c3.AutoIncrementStep);
            Assert.AreEqual(null, c3.DefaultValue);
            Assert.AreEqual(false, c3.IsAutoIncrement);
            Assert.AreEqual(true, c3.IsNullable);
            Assert.AreEqual(32, c3.Length);
            Assert.AreEqual(null, c3.NewName);
            Assert.AreEqual(null, c3.Precision);
            Assert.AreEqual(null, c3.Scale);
            Assert.AreEqual(Modifier.Add, c3.Modifier);

            Assert.AreEqual(c4, Target.Columns[3]);
            Assert.AreEqual("MyColumn4", c4.Name);
            Assert.AreEqual(SqlTypes.Decimal, c4.DataType);
            Assert.AreEqual(null, c4.AutoIncrementSeed);
            Assert.AreEqual(null, c4.AutoIncrementStep);
            Assert.AreEqual(null, c4.DefaultValue);
            Assert.AreEqual(false, c4.IsAutoIncrement);
            Assert.AreEqual(true, c4.IsNullable);
            Assert.AreEqual(null, c4.Length);
            Assert.AreEqual(null, c4.NewName);
            Assert.AreEqual(2, c4.Precision);
            Assert.AreEqual(10, c4.Scale);
            Assert.AreEqual(Modifier.Add, c4.Modifier);

            Assert.AreEqual(c5, Target.Columns[4]);
            Assert.AreEqual("MyColumn5", c5.Name);
            Assert.AreEqual(SqlTypes.NVarChar, c5.DataType);
            Assert.AreEqual(null, c5.AutoIncrementSeed);
            Assert.AreEqual(null, c5.AutoIncrementStep);
            Assert.AreEqual(null, c5.DefaultValue);
            Assert.AreEqual(false, c5.IsAutoIncrement);
            Assert.AreEqual(true, c5.IsNullable);
            Assert.AreEqual(64, c5.Length);
            Assert.AreEqual(null, c5.NewName);
            Assert.AreEqual(null, c5.Precision);
            Assert.AreEqual(null, c5.Scale);
            Assert.AreEqual(Modifier.Add, c5.Modifier);

            Assert.AreEqual(c6, Target.Columns[5]);
            Assert.AreEqual("MyColumn6", c6.Name);
            Assert.AreEqual(SqlTypes.Decimal, c6.DataType);
            Assert.AreEqual(null, c6.AutoIncrementSeed);
            Assert.AreEqual(null, c6.AutoIncrementStep);
            Assert.AreEqual(null, c6.DefaultValue);
            Assert.AreEqual(false, c6.IsAutoIncrement);
            Assert.AreEqual(true, c6.IsNullable);
            Assert.AreEqual(null, c6.Length);
            Assert.AreEqual(null, c6.NewName);
            Assert.AreEqual(4, c6.Precision);
            Assert.AreEqual(18, c6.Scale);
            Assert.AreEqual(Modifier.Add, c6.Modifier);
        }       

        /// <summary>
        /// Checks that the constructor initializes all properties appropiatly.
        /// </summary>
        [TestMethod]
        public void TableConstructorTest()
        {
            Database db = new Database(null);
            Table t = new Table(db, "MyTable", Modifier.Drop);

            Assert.AreEqual(db, t.Database);
            Assert.AreEqual("MyTable", t.Name);
            Assert.AreEqual(Modifier.Drop, t.Modifier);
            Assert.AreEqual(0, t.Columns.Count);
        }
    }
}
