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

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMigrations.Shell.Options;

namespace NMigrations.Test
{    
    [TestClass]
    public class OptionsParserTest : BaseTest
    {
        #region Private Types

        private class CommandLineOptions
        {
            public bool Flag
            {
                get;
                set;
            }

            [OptionDefault("DefaultValue")]
            public string StringOption
            {
                get;
                set;
            }

            [OptionName("mandatoryStringOption", true)]
            public string MandatoryStringOption
            {
                get;
                set;
            }

            public string[] ArrayOption
            {
                get;
                set;
            }

            public int? NullableInteger
            {
                get;
                set;
            }

            public int MandatoryInteger
            {
                get;
                set;
            }
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>The target.</value>
        private OptionsParser_Accessor<CommandLineOptions> Target
        {
            get;
            set;
        }

        #endregion

        #region Initialize Test

        [TestInitialize]
        public void InitializeTarget()
        {
            Target = new OptionsParser_Accessor<CommandLineOptions>();
        }

        #endregion

        /// <summary>
        /// Checks that the <see cref="GetOptions"/> method returns the appropriate
        /// description of the valid command line options.
        /// </summary>
        [TestMethod]
        public void GetOptionsTest()
        {
            var options = Target.GetOptions();

            Assert.AreEqual(6, options.Length);

            var flagOption = options.FirstOrDefault(o => o.Name == "flag");
            Assert.IsNotNull(flagOption);
            Assert.IsTrue(flagOption.IsRequired);
            Assert.AreEqual(typeof(bool), flagOption.Type);
            Assert.IsNull(flagOption.DefaultValue);
            Assert.IsFalse(flagOption.AllowMultiple);

            var stringOption = options.FirstOrDefault(o => o.Name == "stringOption");
            Assert.IsNotNull(stringOption);
            Assert.IsFalse(stringOption.IsRequired);
            Assert.AreEqual(typeof(string), stringOption.Type);
            Assert.AreEqual("DefaultValue", stringOption.DefaultValue);
            Assert.IsFalse(stringOption.AllowMultiple);

            var mandatoryStringOption = options.FirstOrDefault(o => o.Name == "mandatoryStringOption");
            Assert.IsNotNull(mandatoryStringOption);
            Assert.IsTrue(mandatoryStringOption.IsRequired);
            Assert.AreEqual(typeof(string), mandatoryStringOption.Type);
            Assert.IsNull(mandatoryStringOption.DefaultValue);
            Assert.IsFalse(mandatoryStringOption.AllowMultiple);

            var arrayOption = options.FirstOrDefault(o => o.Name == "arrayOption");
            Assert.IsNotNull(arrayOption);
            Assert.IsFalse(arrayOption.IsRequired);
            Assert.AreEqual(typeof(string), arrayOption.Type);
            Assert.IsNull(arrayOption.DefaultValue);
            Assert.IsTrue(arrayOption.AllowMultiple);

            var nullableInteger = options.FirstOrDefault(o => o.Name == "nullableInteger");
            Assert.IsNotNull(nullableInteger);
            Assert.IsFalse(nullableInteger.IsRequired);
            Assert.AreEqual(typeof(int), nullableInteger.Type);
            Assert.IsNull(nullableInteger.DefaultValue);
            Assert.IsFalse(nullableInteger.AllowMultiple);

            var mandatoryInteger = options.FirstOrDefault(o => o.Name == "mandatoryInteger");
            Assert.IsNotNull(mandatoryInteger);
            Assert.IsTrue(mandatoryInteger.IsRequired);
            Assert.AreEqual(typeof(int), mandatoryInteger.Type);
            Assert.IsNull(mandatoryInteger.DefaultValue);
            Assert.IsFalse(mandatoryInteger.AllowMultiple);
        }

        /// <summary>
        /// Checks that the <see cref="Parse"/> method returns the
        /// appropriate object model of the command line arguments.
        /// </summary>
        [TestMethod]
        public void ParseTest()
        {
            var result = Target.Parse(new string[]
            {
                "/flag",
                "/arrayoption", "val1",
                "/arrayoption", "val2",
                "/mandatoryInteger", "492",
                "/mandatoryStringOption", "mystring"
            });

            Assert.IsTrue(result.Flag);
            Assert.AreEqual(2, result.ArrayOption.Length);
            Assert.AreEqual("val1", result.ArrayOption[0]);
            Assert.AreEqual("val2", result.ArrayOption[1]);
            Assert.AreEqual(492, result.MandatoryInteger);
            Assert.AreEqual("mystring", result.MandatoryStringOption);
            Assert.IsNull(result.NullableInteger);
            Assert.AreEqual("DefaultValue", result.StringOption);
        }
    }
}