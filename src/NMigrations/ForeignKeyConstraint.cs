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
    /// Represents a foreign key constraint.
    /// </summary>
    public class ForeignKeyConstraint : Constraint
    {
        #region Internal Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ForeignKeyConstraint"/> class.
        /// </summary>
        /// <param name="table">The <see cref="Table"/> the constraint belongs to.</param>
        /// <param name="name">The name of the constraint.</param>
        /// <param name="modifier">The modifier.</param>
        internal ForeignKeyConstraint(Table table, string name, Modifier modifier)
            : base(table, name, modifier)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// DELETE statements are restricted if referential integrity is affected.
        /// </summary>
        /// <returns>The constraint.</returns>
        public ForeignKeyConstraint Restrict()
        {
            Propagation = ForeignKeyConstraintPropagation.Restricted;
            return this;
        }

        /// <summary>
        /// DELETE statements are cascaded to related tables to preserve referential integrity.
        /// </summary>
        /// <returns></returns>
        public ForeignKeyConstraint Cascade()
        {
            Propagation = ForeignKeyConstraintPropagation.Cascaded;
            return this;
        }

        /// <summary>
        /// Related columns are nullified on DELETE statements to preserve referential integrity.
        /// </summary>
        /// <returns></returns>
        public ForeignKeyConstraint Nullify()
        {
            Propagation = ForeignKeyConstraintPropagation.Nullified;
            return this;
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the column names that refer to 
        /// another <typeparamref name="Table"/>'s key.
        /// </summary>
        /// <value>The column names.</value>
        internal string[] ColumnNames
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the related <see cref="Table"/>.
        /// </summary>
        /// <value>The name of the related table.</value>
        internal string RelatedTableName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the related <see cref="Column"/> names.
        /// </summary>
        /// <value>The related column names.</value>
        internal string[] RelatedColumnNames
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the propagation model.
        /// </summary>
        /// <value>The propagation model.</value>
        internal ForeignKeyConstraintPropagation Propagation
        {
            get;
            set;
        }

        #endregion
    }
}