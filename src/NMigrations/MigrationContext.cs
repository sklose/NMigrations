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

using System.Data.Common;
using NMigrations.Sql;

namespace NMigrations
{
    /// <summary>
    /// Stores context information about a running migration.
    /// </summary>
    public class MigrationContext
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Context"/> class.
        /// </summary>
        protected internal MigrationContext()
        {

        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the database connection.
        /// </summary>
        /// <value>The database connection.</value>
        public virtual DbConnection Connection
        {
            get;
            internal protected set;
        }

        /// <summary>
        /// Gets the connetion string.
        /// </summary>
        /// <value>The connetion string.</value>
        public virtual string ConnetionString
        {
            get;
            internal protected set;
        }

        /// <summary>
        /// Gets the invariant provider name.
        /// </summary>
        /// <value>The invariant provider name.</value>
        public virtual string ProviderInvariantName
        {
            get;
            internal protected set;
        }

        /// <summary>
        /// Gets or sets the SQL provider.
        /// </summary>
        /// <value>The SQL provider.</value>
        public virtual ISqlProvider SqlProvider
        {
            get;
            internal protected set;
        }

        /// <summary>
        /// Gets or sets the SQL processor.
        /// </summary>
        /// <value>The SQL processor.</value>
        public virtual ISqlProcessor SqlProcessor
        {
            get;
            internal protected set;
        }

        #endregion
    }
}
