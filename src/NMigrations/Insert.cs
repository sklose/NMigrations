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

using System.Collections.Generic;

namespace NMigrations
{
    /// <summary>
    /// Represents an INSERT statement.
    /// </summary>
    public class Insert : Element
    {
        #region Internal Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Insert"/> class.
        /// </summary>
        /// <param name="table">The table the insert belongs to.</param>
        /// <param name="row">The row to insert as key/value pairs.</param>
        internal Insert(Table table, Dictionary<string, object> row) : base(null, Modifier.Add)
        {
            Row = row;
            Table = table;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Insert"/> class.
        /// </summary>
        /// <param name="table">The table the insert belongs to.</param>
        /// <param name="row">The row to insert as key/value pairs in the form of properties of an anonymous object.</param>
        internal Insert(Table table, object row) 
            : this(table, row.ToDictionary())
        {

        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the row to insert as key/value pairs.
        /// </summary>
        /// <value>The row.</value>
        internal Dictionary<string, object> Row
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the table the insert belongs to
        /// </summary>
        /// <value>The table.</value>
        internal Table Table
        {
            get;
            set;
        }

        #endregion
    }
}