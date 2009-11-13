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
    internal class MockDbParameter : DbParameter
    {
        /// <summary>
        /// Gets or sets the <see cref="T:System.Data.DbType"/> of the parameter.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// One of the <see cref="T:System.Data.DbType"/> values. The default is <see cref="F:System.Data.DbType.String"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">
        /// The property is not set to a valid <see cref="T:System.Data.DbType"/>.
        /// </exception>
        public override DbType DbType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the parameter is input-only, output-only, bidirectional, or a stored procedure return value parameter.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// One of the <see cref="T:System.Data.ParameterDirection"/> values. The default is Input.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">
        /// The property is not set to one of the valid <see cref="T:System.Data.ParameterDirection"/> values.
        /// </exception>
        public override ParameterDirection Direction
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the parameter accepts null values.
        /// </summary>
        /// <value></value>
        /// <returns>true if null values are accepted; otherwise false. The default is false.
        /// </returns>
        public override bool IsNullable
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the <see cref="T:System.Data.Common.DbParameter"/>.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The name of the <see cref="T:System.Data.Common.DbParameter"/>. The default is an empty string ("").
        /// </returns>
        public override string ParameterName
        {
            get;
            set;
        }

        /// <summary>
        /// Resets the DbType property to its original settings.
        /// </summary>
        public override void ResetDbType()
        {
            
        }

        /// <summary>
        /// Gets or sets the maximum size, in bytes, of the data within the column.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The maximum size, in bytes, of the data within the column. The default value is inferred from the parameter value.
        /// </returns>
        public override int Size
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the source column mapped to the <see cref="T:System.Data.DataSet"/> and used for loading or returning the <see cref="P:System.Data.Common.DbParameter.Value"/>.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The name of the source column mapped to the <see cref="T:System.Data.DataSet"/>. The default is an empty string.
        /// </returns>
        public override string SourceColumn
        {
            get;
            set;
        }

        /// <summary>
        /// Sets or gets a value which indicates whether the source column is nullable. This allows <see cref="T:System.Data.Common.DbCommandBuilder"/> to correctly generate Update statements for nullable columns.
        /// </summary>
        /// <value></value>
        /// <returns>true if the source column is nullable; false if it is not.
        /// </returns>
        public override bool SourceColumnNullMapping
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="T:System.Data.DataRowVersion"/> to use when you load <see cref="P:System.Data.Common.DbParameter.Value"/>.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// One of the <see cref="T:System.Data.DataRowVersion"/> values. The default is Current.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">
        /// The property is not set to one of the <see cref="T:System.Data.DataRowVersion"/> values.
        /// </exception>
        public override DataRowVersion SourceVersion
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value of the parameter.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// An <see cref="T:System.Object"/> that is the value of the parameter. The default value is null.
        /// </returns>
        public override object Value
        {
            get;
            set;
        }
    }
}