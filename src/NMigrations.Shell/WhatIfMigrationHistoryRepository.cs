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

using NMigrations.Repository;

namespace NMigrations.Shell
{
    /// <summary>
    /// Overrides some methods of the original <see cref="MigrationHistoryRepository"/>
    /// to ensure that no changes to the database are made.
    /// </summary>
    internal class WhatIfMigrationHistoryRepository : MigrationHistoryRepository
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WhatIfMigrationHistoryRepository"/> class.
        /// </summary>
        public WhatIfMigrationHistoryRepository()
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Stores a new migration history record in the database.
        /// </summary>
        /// <param name="context">The migration context.</param>
        /// <param name="item">The item.</param>
        public override void AddItem(MigrationContext context, MigrationHistoryItem item)
        {
            // ignore ...
        }

        /// <summary>
        /// Ensures that the schema for the migration history exists.
        /// </summary>
        /// <param name="context">The context.</param>
        protected override void EnsureMigrationHistorySchemaExists(MigrationContext context)
        {
            // ignore ...
        }

        #endregion
    }
}