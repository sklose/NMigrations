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
    internal class MockDbConnection : DbConnection
    {
        /// <summary>
        /// Starts a database transaction.
        /// </summary>
        /// <param name="isolationLevel">Specifies the isolation level for the transaction.</param>
        /// <returns>
        /// An object representing the new transaction.
        /// </returns>
        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            return new MockDbTransaction(this, isolationLevel);
        }

        /// <summary>
        /// Changes the current database for an open connection.
        /// </summary>
        /// <param name="databaseName">Specifies the name of the database for the connection to use.</param>
        public override void ChangeDatabase(string databaseName)
        {
            
        }

        /// <summary>
        /// Closes the connection to the database. This is the preferred method of closing any open connection.
        /// </summary>
        /// <exception cref="T:System.Data.Common.DbException">
        /// The connection-level error that occurred while opening the connection.
        /// </exception>
        public override void Close()
        {
            
        }

        /// <summary>
        /// Gets or sets the string used to open the connection.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The connection string used to establish the initial connection. The exact contents of the connection string depend on the specific data source for this connection. The default value is an empty string.
        /// </returns>
        public override string ConnectionString
        {
            get;
            set;
        }

        /// <summary>
        /// Creates and returns a <see cref="T:System.Data.Common.DbCommand"/> object associated with the current connection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Data.Common.DbCommand"/> object.
        /// </returns>
        protected override DbCommand CreateDbCommand()
        {
            return new MockDbCommand(this);
        }

        /// <summary>
        /// Gets the name of the database server to which to connect.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The name of the database server to which to connect. The default value is an empty string.
        /// </returns>
        public override string DataSource
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets the name of the current database after a connection is opened, or the database name specified in the connection string before the connection is opened.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The name of the current database or the name of the database to be used after a connection is opened. The default value is an empty string.
        /// </returns>
        public override string Database
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Opens a database connection with the settings specified by the <see cref="P:System.Data.Common.DbConnection.ConnectionString"/>.
        /// </summary>
        public override void Open()
        {
            
        }

        /// <summary>
        /// Gets a string that represents the version of the server to which the object is connected.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The version of the database. The format of the string returned depends on the specific type of connection you are using.
        /// </returns>
        public override string ServerVersion
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets a string that describes the state of the connection.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The state of the connection. The format of the string returned depends on the specific type of connection you are using.
        /// </returns>
        public override ConnectionState State
        {
            get { return ConnectionState.Open; }
        }
    }
}