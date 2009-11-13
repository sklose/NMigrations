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

namespace NMigrations.Sql
{
    /// <summary>
    /// Processes SQL statements by immediately executing them in the RDBMS.
    /// </summary>
    public class DatabaseSqlProcessor : ISqlProcessor
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseSqlProcessor"/> class.
        /// </summary>
        public DatabaseSqlProcessor()
        {

        }

        #endregion

        #region ISqlProcessor Members

        /// <summary>
        /// Processes a <paramref name="sql"/> statement that was
        /// issued by the migration model (<see cref="Database"/>).
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="sql">The SQL.</param>
        public void ProcessMigrationStatement(MigrationContext context, string sql)
        {
            using (var cmd = context.Connection.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Processes a <paramref name="sql"/> statement that was
        /// issued by the <see cref="Engine"/>.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="sql">The SQL.</param>
        public void ProcessEngineStatement(MigrationContext context, string sql)
        {
            ProcessMigrationStatement(context, sql);
        }

        #endregion
    }
}