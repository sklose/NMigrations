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

namespace NMigrations.Test
{   
    [TestClass]
    public class IMigrationExtensionsTest : BaseTest
    {
        [Migration(7)]
        private class MyTestMigration : IMigration
        {
            #region IMigration Members

            public void Up(Database db)
            {
                throw new System.NotImplementedException();
            }

            public void Down(Database db)
            {
                throw new System.NotImplementedException();
            }

            #endregion
        }

        [Migration(12)]
        private class MyTestWithoutExtension : IMigration
        {
            #region IMigration Members

            public void Up(Database db)
            {
                throw new System.NotImplementedException();
            }

            public void Down(Database db)
            {
                throw new System.NotImplementedException();
            }

            #endregion
        }

        /// <summary>
        /// Checks that the <see cref="GetVersion"/> method returns the
        /// version stored in the <see cref="MigrationAttribute"/>.
        /// </summary>
        [TestMethod()]
        public void GetVersionTest()
        {
            IMigration migration = new MyTestMigration();
            Assert.AreEqual(7, migration.GetVersion());

            migration = new MyTestWithoutExtension();
            Assert.AreEqual(12, migration.GetVersion());
        }

        /// <summary>
        /// Checks that the <see cref="GetName"/> method returns the name
        /// of the class without "Migration" suffix.
        /// </summary>
        [TestMethod()]
        public void GetNameTest()
        {
            IMigration migration = new MyTestMigration();
            Assert.AreEqual("MyTest", migration.GetName());

            migration = new MyTestWithoutExtension();
            Assert.AreEqual("MyTestWithoutExtension", migration.GetName());
        }
    }
}
