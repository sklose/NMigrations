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
    public class ObjectExtensionsTest : BaseTest
    {
        /// <summary>
        /// Checks if an anonymous type is converted to a dictionary correctly.
        /// </summary>
        [TestMethod]
        public void ConvertAnonymousTypeToDictionaryTest()
        {
            object obj = new
            {
                StringValue = "Hello World",
                IntValue = 3,
                DateValue = new DateTime(2000, 1, 1)
            };

            var dictionary = obj.ToDictionary();

            Assert.AreEqual(3, dictionary.Count, "invalid number of elements in dictionary");
            Assert.IsTrue(dictionary.ContainsKey("StringValue"));
            Assert.IsTrue(dictionary.ContainsKey("IntValue"));
            Assert.IsTrue(dictionary.ContainsKey("DateValue"));
            Assert.AreEqual("Hello World", dictionary["StringValue"]);
            Assert.AreEqual(3, dictionary["IntValue"]);
            Assert.AreEqual(new DateTime(2000, 1, 1), dictionary["DateValue"]);
        }
    }
}
