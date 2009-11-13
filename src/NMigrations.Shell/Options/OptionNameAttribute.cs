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

namespace NMigrations.Shell.Options
{
    /// <summary>
    /// Defines the name for a command line <see cref="Option"/> and
    /// defines if the <see cref="Option"/> is required.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    internal class OptionNameAttribute : Attribute
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionNameAttribute"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="isRequired">if set to <c>true</c> the command line option is mandatory.</param>
        public OptionNameAttribute(string name, bool isRequired)
        {
            Name = name;
            IsRequired = isRequired;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the option is required.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the option is required; otherwise, <c>false</c>.
        /// </value>
        public bool IsRequired
        {
            get;
            private set;
        }

        #endregion
    }
}