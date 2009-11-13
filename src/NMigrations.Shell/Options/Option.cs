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
using System.Reflection;

namespace NMigrations.Shell.Options
{
    /// <summary>
    /// Describes a command line option.
    /// </summary>
    internal class Option
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Option"/> class.
        /// </summary>
        public Option()
        {

        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the property.
        /// </summary>
        /// <value>The property.</value>
        public PropertyInfo Property
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the option is required.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the option is required; otherwise, <c>false</c>.
        /// </value>
        public bool IsRequired
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the option can be applied multiple times.
        /// </summary>
        /// <value><c>true</c> if the option can be applied multiple times; otherwise, <c>false</c>.</value>
        public bool AllowMultiple
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>The default value.</value>
        public object DefaultValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public Type Type
        {
            get;
            set;
        }

        #endregion
    }
}