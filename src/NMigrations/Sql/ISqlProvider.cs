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
using NMigrations.Core;

namespace NMigrations.Sql
{
    /// <summary>
    /// Defines the interface for SQL provider classes.
    /// </summary>
    public interface ISqlProvider
    {
        /// <summary>
        /// Generates the SQL commands for the specified <paramref name="database"/>.
        /// </summary>
        /// <param name="database">The database.</param>
        /// <returns>An enumeration of SQL commands.</returns>
        IEnumerable<string> GenerateSqlCommands(Database database);

        /// <summary>
        /// Gets the SQL command that separates multiple SQL queries in one SQL script file of each other.
        /// </summary>
        /// <returns>The separator.</returns>
        string GetQuerySeparator();

        /// <summary>
        /// Builds the name for a primary key constraint.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columnNames">The column names.</param>
        /// <returns>The primary key name.</returns>
        string GetPrimaryKeyConstraintName(string tableName, string[] columnNames);

        /// <summary>
        /// Builds the name for a foreign key constraint.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columnNames">The column names.</param>
        /// <param name="relatedTableName">Name of the related table.</param>
        /// <param name="relatedColumnNames">The related column names.</param>
        /// <returns>The foreign key name.</returns>
        string GetForeignKeyConstraintName(string tableName, string[] columnNames, string relatedTableName, string[] relatedColumnNames);

        /// <summary>
        /// Builds the name for a unique constraint.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columnNames">The column names.</param>
        /// <returns>The unqiue constraint name.</returns>
        string GetUniqueConstraintName(string tableName, string[] columnNames);

        /// <summary>
        /// Builds the name for an index.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columnNames">The column names.</param>
        /// <returns>The index name.</returns>
        string GetIndexName(string tableName, string[] columnNames);
    }
}