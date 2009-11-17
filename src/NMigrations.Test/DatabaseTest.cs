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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NMigrations.Test
{
    [TestClass]
    public class DatabaseTest : BaseTest
    {
        #region Private Properties

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>The target.</value>
        private Database Target
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
            Target = new Database(context);
        }

        #endregion

        /// <summary>
        /// Checks that the OnFlushChanges method raises the FlushChanges event
        /// and passes the right arguments.
        /// </summary>
        [TestMethod]
        public void OnFlushChangesTest()
        {            
            EventArgs args = new EventArgs();

            bool triggered = false;
            Target.FlushChanges += delegate(object sender, EventArgs e)
            {
                Assert.AreEqual(Target, sender, "database has to be sender");
                Assert.AreEqual(args, e, "EventArgs differ");
                triggered = true;
            };

            Target.OnFlushChanges(args);
            Assert.IsTrue(triggered, "event didn't fire");
        }

        /// <summary>
        /// Checks that the FlushChanges event is triggered.
        /// </summary>
        [TestMethod]
        public void FlushTest()
        {
            bool triggered = false;
            Target.FlushChanges += delegate(object sender, EventArgs e)
            {
                triggered = true;
            };

            Target.Flush();
            Assert.IsTrue(triggered, "event didn't fire");
        }

        /// <summary>
        /// Checks that a <see cref="Table"/> is added to the migration queue.
        /// </summary>
        [TestMethod]
        public void DropTableTest()
        {
            Target.DropTable("MyTable");
            Assert.AreEqual(1, Target.MigrationSteps.Count, "invalid number of migration steps");

            Element e = Target.MigrationSteps.Dequeue();
            Assert.IsInstanceOfType(e, typeof(Table));
            Table t = e as Table;

            Assert.AreEqual("MyTable", t.Name);
            Assert.AreEqual(Modifier.Drop, t.Modifier);
        }

        /// <summary>
        /// Checks that a <see cref="Table"/> is added to the migration queue.
        /// </summary>
        [TestMethod]
        public void AlterTableTest()
        {
            Target.AlterTable("MyTable");
            Assert.AreEqual(1, Target.MigrationSteps.Count, "invalid number of migration steps");

            Element e = Target.MigrationSteps.Dequeue();
            Assert.IsInstanceOfType(e, typeof(Table));
            Table t = e as Table;

            Assert.AreEqual("MyTable", t.Name);
            Assert.AreEqual(Modifier.Alter, t.Modifier);
        }

        /// <summary>
        /// Checks that a <see cref="Table"/> is added to the migration queue.
        /// </summary>
        [TestMethod]
        public void AddTableTest()
        {
            Target.AddTable("MyTable");
            Assert.AreEqual(1, Target.MigrationSteps.Count, "invalid number of migration steps");

            Element e = Target.MigrationSteps.Dequeue();
            Assert.IsInstanceOfType(e, typeof(Table));
            Table t = e as Table;

            Assert.AreEqual("MyTable", t.Name);
            Assert.AreEqual(Modifier.Add, t.Modifier);
        }

        /// <summary>
        /// Checks that the constructor initializes the <see cref="MigrationSteps"/>
        /// queue and stores the <see cref="MigrationContext"/>.
        /// </summary>
        [TestMethod]
        public void DatabaseConstructorTest()
        {
            MigrationContext context = new MigrationContext();
            Database target = new Database(context);

            Assert.AreEqual(context, target.Context);
            Assert.IsNotNull(target.MigrationSteps);
            Assert.AreEqual(0, target.MigrationSteps.Count);
        }
    }
}
