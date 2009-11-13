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
    /// Defines a default value for a command line <see cref="Option"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    internal class OptionDefaultAttribute : Attribute
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionDefaultAttribute"/> class.
        /// </summary>
        /// <param name="value">The default value.</param>
        public OptionDefaultAttribute(object value)
        {
            Value = value;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the default value.
        /// </summary>
        /// <value>The default value.</value>
        public object Value
        {
            get;
            private set;
        }

        #endregion
    }
}