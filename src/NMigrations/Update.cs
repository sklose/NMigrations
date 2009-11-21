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
    /// Represents a simple UPDATE statement.
    /// </summary>
    public class Update : Element
    {
        #region Internal Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Update"/> class.
        /// </summary>
        /// <param name="table">The table the update belongs to.</param>
        /// <param name="set">The new column values as key/value pairs.</param>
        /// <param name="where">The update's where clause key/value pairs.</param>
        internal Update(Table table, Dictionary<string, object> set, Dictionary<string, object> where)
            : base(null, Modifier.Alter)
        {
            Set = set;
            Where = where;
            Table = table;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Update"/> class.
        /// </summary>
        /// <param name="table">The table the insert belongs to.</param>
        /// <param name="set">The columns to update as key/value pairs in form of properties of an anonymous object.</param>
        /// <param name="where">The update's where clause as key/value pairs in form of properties of an anonymous object.</param>
        internal Update(Table table, object set, object where)
            : this(table, set.ToDictionary(), where.ToDictionary())
        {

        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets update's where clause as key/value pairs.
        /// </summary>
        /// <value>The where clause.</value>
        internal Dictionary<string, object> Where
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets new column values as key/value pairs.
        /// </summary>
        /// <value>The new values.</value>
        internal Dictionary<string, object> Set
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