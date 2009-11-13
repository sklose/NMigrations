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

using NMigrations.Core;

namespace NMigrations.Repository
{
    /// <summary>
    /// Defines the interface for migration history repositories.
    /// </summary>
    public interface IMigrationHistoryRepository
    {
        /// <summary>
        /// Retrieves information about all migrations that have
        /// been applied to the database so far.
        /// </summary>
        /// <param name="context">The migration context.</param>
        /// <returns>The migration history.</returns>
        MigrationHistoryItem[] RetrieveHistory(MigrationContext context);

        /// <summary>
        /// Stores a new migration history record in the database.
        /// </summary>
        /// <param name="context">The migration context.</param>
        /// <param name="item">The item.</param>
        void AddItem(MigrationContext context, MigrationHistoryItem item);
    }
}