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

namespace NMigrations
{
    /// <summary>
    /// The base class for all schema operations.
    /// </summary>
    public abstract class Element
    {
        #region Protected Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Element"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="modifier">The modifier.</param>
        internal Element(string name, Modifier modifier)
        {
            Name = name;
            Modifier = modifier;
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        internal string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the modifier.
        /// </summary>
        /// <value>The modifier.</value>
        internal Modifier Modifier
        {
            get;
            set;
        }

        #endregion
    }
}