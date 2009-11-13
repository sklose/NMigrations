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
    /// Represents a unique constraint.
    /// </summary>
    public class UniqueConstraint : Constraint
    {
        #region Internal Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueConstraint"/> class.
        /// </summary>
        /// <param name="table">The <see cref="Table"/> the constraint belongs to.</param>
        /// <param name="name">The name of the constraint.</param>
        /// <param name="modifier">The modifier.</param>
        internal UniqueConstraint(Table table, string name, Modifier modifier)
            : base(table, name, modifier)
        {

        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the column names that in conjunction need to be unique.
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