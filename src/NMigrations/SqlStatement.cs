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
    /// Represents an a SQL statement.
    /// </summary>
    public class SqlStatement : Element
    {
        #region Internal Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlStatement"/> class.
        /// </summary>
        /// <param name="database">The <see cref="Database"/> this SQL statement belongs to.</param>
        /// <param name="sql">The SQL statement.</param>
        internal SqlStatement(Database database, string sql)
            : base(null, Modifier.Add)
        {
            Database = database;
            Sql = sql;
        }

        #endregion

        #region Internal Properies

        /// <summary>
        /// Gets or sets the <see cref="Database"/> this SQL statement belongs to.
        /// </summary>
        /// <value>The database.</value>
        internal Database Database
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the SQL statement
        /// </summary>
        /// <value>The SQL statement.</value>
        internal string Sql
        {
            get;
            set;
        }

        #endregion
    }
}
