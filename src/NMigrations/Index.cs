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

namespace NMigrations
{
    /// <summary>
    /// Represents an index.
    /// </summary>
    public class Index : Element
    {
        #region Internal Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Index"/> class.
        /// </summary>
        /// <param name="table">The <see cref="Table"/> this index belongs to.</param>
        /// <param name="name">The name of the index.</param>
        /// <param name="modifier">The modifier.</param>
        internal Index(Table table, string name, Modifier modifier)
            : base(name, modifier)
        {
            Table = table;
        }

        #endregion

        #region Internal Properies

        /// <summary>
        /// Gets or sets the <see cref="Table"/> this index belongs to.
        /// </summary>
        /// <value>The table.</value>
        internal Table Table
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the column names that are enclosed in the index.
        /// </summary>
        /// <value>The column names.</value>
        internal string[] ColumnNames
        {
            get;
            set;
        }

        #endregion
    }
}
