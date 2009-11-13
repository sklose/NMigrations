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
    internal class MockDbCommand : DbCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MockDbCommand"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public MockDbCommand(DbConnection connection)
        {
            Connection = connection;
        }

        /// <summary>
        /// Attempts to cancels the execution of a <see cref="T:System.Data.Common.DbCommand"/>.
        /// </summary>
        public override void Cancel()
        {
            
        }

        /// <summary>
        /// Gets or sets the text command to run against the data source.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The text command to execute. The default value is an empty string ("").
        /// </returns>
        public override string CommandText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the wait time before terminating the attempt to execute a command and generating an error.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The time in seconds to wait for the command to execute.
        /// </returns>
        public override int CommandTimeout
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates or specifies how the <see cref="P:System.Data.Common.DbCommand.CommandText"/> property is interpreted.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// One of the <see cref="T:System.Data.CommandType"/> values. The default is Text.
        /// </returns>
        public override CommandType CommandType
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a new instance of a <see cref="T:System.Data.Common.DbParameter"/> object.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Data.Common.DbParameter"/> object.
        /// </returns>
        protected override DbParameter CreateDbParameter()
        {
            return new MockDbParameter();
        }

        /// <summary>
        /// Gets or sets the <see cref="T:System.Data.Common.DbConnection"/> used by this <see cref="T:System.Data.Common.DbCommand"/>.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The connection to the data source.
        /// </returns>
        protected override DbConnection DbConnection
        {
            get;
            set;
        }

        protected override DbParameterCollection DbParameterCollection
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets or sets the <see cref="P:System.Data.Common.DbCommand.DbTransaction"/> within which this <see cref="T:System.Data.Common.DbCommand"/> object executes.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The transaction within which a Command object of a .NET Framework data provider executes. The default value is a null reference (Nothing in Visual Basic).
        /// </returns>
        protected override DbTransaction DbTransaction
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the command object should be visible in a customized interface control.
        /// </summary>
        /// <value></value>
        /// <returns>true, if the command object should be visible in a control; otherwise false. The default is true.
        /// </returns>
        public override bool DesignTimeVisible
        {
            get;
            set;
        }

        /// <summary>
        /// Executes the command text against the connection.
        /// </summary>
        /// <param name="behavior">An instance of <see cref="T:System.Data.CommandBehavior"/>.</param>
        /// <returns>
        /// A <see cref="T:System.Data.Common.DbDataReader"/>.
        /// </returns>
        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            return null;
        }

        /// <summary>
        /// Executes a SQL statement against a connection object.
        /// </summary>
        /// <returns>The number of rows affected.</returns>
        public override int ExecuteNonQuery()
        {
            return 0;
        }

        /// <summary>
        /// Executes the query and returns the first column of the first row in the result set returned by the query. All other columns and rows are ignored.
        /// </summary>
        /// <returns>
        /// The first column of the first row in the result set.
        /// </returns>
        public override object ExecuteScalar()
        {
            return null;
        }

        /// <summary>
        /// Creates a prepared (or compiled) version of the command on the data source.
        /// </summary>
        public override void Prepare()
        {
            
        }

        /// <summary>
        /// Gets or sets how command results are applied to the <see cref="T:System.Data.DataRow"/> when used by the Update method of a <see cref="T:System.Data.Common.DbDataAdapter"/>.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// One of the <see cref="T:System.Data.UpdateRowSource"/> values. The default is Both unless the command is automatically generated. Then the default is None.
        /// </returns>
        public override UpdateRowSource UpdatedRowSource
        {
            get;
            set;
        }
    }
}