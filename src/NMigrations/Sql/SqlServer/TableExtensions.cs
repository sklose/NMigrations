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

namespace NMigrations.Sql.SqlServer
{
    /// <summary>
    /// Contains extension methods for the <see cref="Table"/> class for Microsoft SQL Server.
    /// </summary>
    public static class TableExtensions
    {
        /// <summary>
        /// Enables identity insert for the specified <paramref name="table"/>.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns>The table.</returns>
        public static Table EnableIdentityInsert(this Table table)
        {
            table.Database.ExecuteSql(string.Format("SET IDENTITY_INSERT [{0}] ON", table.Name));
            return table;
        }

        /// <summary>
        /// Disables identity insert for the specified <paramref name="table"/>.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns>The table.</returns>
        public static Table DisableIdentityInsert(this Table table)
        {
            table.Database.ExecuteSql(string.Format("SET IDENTITY_INSERT [{0}] OFF", table.Name));
            return table;
        }
    }
}