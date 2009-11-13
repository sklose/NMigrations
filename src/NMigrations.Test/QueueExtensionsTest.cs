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
using NMigrations.Sql;

namespace NMigrations.Test
{
    [TestClass]
    public class QueueExtensionsTest : BaseTest
    {
        /// <summary>
        /// Creates a queue with elements from "string 1" to "string 10".
        /// </summary>
        /// <returns>The queue.</returns>
        private Queue<string> CreateQueue()
        {
            var q = new Queue<string>();
            for (int i = 1; i <= 10; i++)
            {
                q.Enqueue(string.Format("string {0}", i));
            }
            return q;
        }

        /// <summary>
        /// Checks that the <see cref="Remove"/> method removes the appropriate item.
        /// </summary>
        [TestMethod]
        public void RemoveTest()
        {
            //
            // Remove inner item
            //
            var queue = CreateQueue();
            queue.Remove("string 3");
            string[] items = queue.ToArray();
            Assert.AreEqual(9, items.Length);
            Assert.AreEqual("string 1", items[0]);
            Assert.AreEqual("string 2", items[1]);
            Assert.AreEqual("string 4", items[2]);
            Assert.AreEqual("string 5", items[3]);
            Assert.AreEqual("string 6", items[4]);
            Assert.AreEqual("string 7", items[5]);
            Assert.AreEqual("string 8", items[6]);
            Assert.AreEqual("string 9", items[7]);
            Assert.AreEqual("string 10", items[8]);

            //
            // Remove first item
            //
            queue = CreateQueue();
            queue.Remove("string 1");
            items = queue.ToArray();
            Assert.AreEqual(9, items.Length);
            Assert.AreEqual("string 2", items[0]);
            Assert.AreEqual("string 3", items[1]);
            Assert.AreEqual("string 4", items[2]);
            Assert.AreEqual("string 5", items[3]);
            Assert.AreEqual("string 6", items[4]);
            Assert.AreEqual("string 7", items[5]);
            Assert.AreEqual("string 8", items[6]);
            Assert.AreEqual("string 9", items[7]);
            Assert.AreEqual("string 10", items[8]);

            //
            // Remove last item
            //
            queue = CreateQueue();
            queue.Remove("string 10");
            items = queue.ToArray();
            Assert.AreEqual(9, items.Length);
            Assert.AreEqual("string 1", items[0]);
            Assert.AreEqual("string 2", items[1]);
            Assert.AreEqual("string 3", items[2]);
            Assert.AreEqual("string 4", items[3]);
            Assert.AreEqual("string 5", items[4]);
            Assert.AreEqual("string 6", items[5]);
            Assert.AreEqual("string 7", items[6]);
            Assert.AreEqual("string 8", items[7]);
            Assert.AreEqual("string 9", items[8]);
        }
    }
}