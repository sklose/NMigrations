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
using System.Collections.Generic;

using NMigrations.Sql;
using NMigrations.Core;

namespace NMigrations
{
    /// <summary>
    /// Represents the database to migrate from one version to another.
    /// </summary>
    public class Database
    {
        #region Internal Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Database"/> class.
        /// </summary>
        /// <param name="context">The engine's context.</param>
        public Database(MigrationContext context)
        {
            Context = context;
            MigrationSteps = new Queue<Element>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a new <see cref="Table"/> with the specified 
        /// <paramref name="name"/> to this <see cref="Database"/>.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The table.</returns>
        public Table AddTable(string name)
        {
            Table t = new Table(this, name, Modifier.Add);
            MigrationSteps.Enqueue(t);
            return t;
        }

        /// <summary>
        /// Drops the <see cref="Table"/> with the specified
        /// <paramref name="name"/> from the <see cref="Database"/>.
        /// </summary>
        /// <param name="name">The name.</param>
        public void DropTable(string name)
        {
            MigrationSteps.Enqueue(new Table(this, name, Modifier.Drop));
        }

        /// <summary>
        /// Alters the <see cref="Table"/> with the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The table.</returns>
        public Table AlterTable(string name)
        {
            Table t = new Table(this, name, Modifier.Alter);
            MigrationSteps.Enqueue(t);
            return t;
        }

        /// <summary>
        /// Executes the specified <paramref name="sql"/> statement.
        /// </summary>
        /// <param name="sql">The SQL statment.</param>
        /// <returns>The SQL statement.</returns>
        public SqlStatement ExecuteSql(string sql)
        {
            SqlStatement stm = new SqlStatement(this, sql);
            MigrationSteps.Enqueue(stm);
            return stm;
        }

        /// <summary>
        /// Immediately writes the changes to the database.
        /// </summary>
        public void Flush()
        {
            OnFlushChanges(new EventArgs());
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the migration steps.
        /// </summary>
        /// <value>The migration steps.</value>
        internal Queue<Element> MigrationSteps
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the engine's context.
        /// </summary>
        /// <value>The engine's context.</value>
        internal MigrationContext Context
        {
            get;
            set;
        }

        #endregion

        #region Internal Events

        /// <summary>
        /// Occurs when the <see cref="Flush"/> method is called.
        /// </summary>
        protected internal event EventHandler<EventArgs> FlushChanges;

        /// <summary>
        /// Raises the <see cref="E:FlushChanges"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected internal void OnFlushChanges(EventArgs e)
        {
            if (FlushChanges != null)
            {
                FlushChanges(this, e);
            }
        }

        #endregion
    }
}