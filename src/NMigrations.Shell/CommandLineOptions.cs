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

using NMigrations.Shell.Options;

namespace NMigrations.Shell
{
    /// <summary>
    /// Represents the command line options.
    /// </summary>
    internal class CommandLineOptions
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineOptions"/> class.
        /// </summary>
        public CommandLineOptions()
        {

        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the a value indicating whether the "whatIf" command line switch is set.
        /// </summary>
        /// <value>
        ///     <c>true</c> if "whatIf" is set; otherwise, <c>false</c>.
        /// </value>
        [OptionName("whatIf", false)]
        [OptionDescription("Doesn't execute any SQL statement, but only prints them to the console")]
        [OptionDefault(false)]
        public bool WhatIf
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the a value indicating whether the "confirm" command line switch is set.
        /// </summary>
        /// <value>
        ///     <c>true</c> if "confirm" is set; otherwise, <c>false</c>.
        /// </value>
        [OptionName("confirm", false)]
        [OptionDescription("Will ask for confirmation before executing a SQL command")]
        [OptionDefault(false)]
        public bool Confirm
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value of the "version" command line option.
        /// </summary>
        /// <value>The value or <c>null</c> if option isn't set.</value>
        [OptionDescription("Specifies the version to migrate to, if argument is missing the latest version is assumed")]
        public long? Version
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value of the "providerName" command line option.
        /// </summary>
        /// <value>The value or its default value.</value>
        [OptionName("providerName", false)]
        [OptionDefault("System.Data.SqlClient")]
        [OptionDescription("The provider invariant name, default is 'System.Data.SqlClient'")]
        public string ProviderInvariantName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value of the "cs" command line option.
        /// </summary>
        /// <value>The value.</value>
        [OptionName("cs", true)]
        [OptionDescription("Connection string to database")]
        public string ConnectionString
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the values of the "assembly" command line options.
        /// </summary>
        /// <value>The values.</value>
        [OptionName("assembly", true)]
        [OptionDescription("The assemblies to scan for migrations, argument may be supplied multiple times")]
        public string[] Assemblies
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value of the "silent" command line option.
        /// </summary>
        /// <value><c>true</c> if silent; otherwise, <c>false</c>.</value>
        [OptionName("silent", false)]
        [OptionDescription("Suppresses the copyright banner on startup and all messages except errors and user inputs")]
        public bool Silent
        {
            get;
            set;
        }

        #endregion
    }
}