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

namespace NMigrations.Core
{
    /// <summary>
    /// Contains event information for the <see cref="Engine.AfterSql"/> event.
    /// </summary>
    public class AfterSqlEventArgs : EventArgs
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AfterSqlEventArgs"/> class.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="success">if set to <c>true</c> the SQL was executed successfully.</param>
        public AfterSqlEventArgs(string sql, bool success)
        {
            Sql = sql;
            Success = success;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the SQL statement.
        /// </summary>
        /// <value>The SQL statement.</value>
        public string Sql
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Sql"/> was executed successfully.
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        public bool Success
        {
            get;
            protected set;
        }

        #endregion
    }
}