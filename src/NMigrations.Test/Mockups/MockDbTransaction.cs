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
using System.Data;
using System.Data.Common;

namespace NMigrations.Test.Mockups
{
    internal class MockDbTransaction : DbTransaction
    {
        private DbConnection connection;
        private IsolationLevel isolationLevel;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockDbTransaction"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="isolationLevel">The isolation level.</param>
        public MockDbTransaction(DbConnection connection, IsolationLevel isolationLevel)
        {
            this.connection = connection;
            this.isolationLevel = isolationLevel;
        }

        /// <summary>
        /// Commits the database transaction.
        /// </summary>
        public override void Commit()
        {
            
        }

        /// <summary>
        /// Specifies the <see cref="T:System.Data.Common.DbConnection"/> object associated with the transaction.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The <see cref="T:System.Data.Common.DbConnection"/> object associated with the transaction.
        /// </returns>
        protected override DbConnection DbConnection
        {
            get { return connection; }
        }

        /// <summary>
        /// Specifies the <see cref="T:System.Data.IsolationLevel"/> for this transaction.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The <see cref="T:System.Data.IsolationLevel"/> for this transaction.
        /// </returns>
        public override IsolationLevel IsolationLevel
        {
            get { return isolationLevel; }
        }

        /// <summary>
        /// Rolls back a transaction from a pending state.
        /// </summary>
        public override void Rollback()
        {
            
        }
    }
}