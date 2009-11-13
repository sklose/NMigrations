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
using System.Linq;

using NMigrations.Sql;

namespace NMigrations
{
    /// <summary>
    /// Represents a column of a table in the database.
    /// </summary>
    public class Column
    {
        #region Internal Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Column"/> class.
        /// </summary>
        /// <param name="table">The parent table.</param>
        /// <param name="name">The column's name.</param>
        /// <param name="modifier">The modifier.</param>
        internal Column(Table table, string name, Modifier modifier)
        {
            Table = table;
            IsNullable = true;
            Name = name;
            Modifier = modifier;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Makes this <see cref="Column"/> an auto increment column.
        /// </summary>
        /// <returns>The column.</returns>
        public Column AutoIncrement()
        {
            IsAutoIncrement = true;
            return this;
        }

        /// <summary>
        /// Makes this <see cref="Column"/> an auto increment column.
        /// </summary>
        /// <param name="seed">The initial value.</param>
        /// <param name="step">The increment step.</param>
        /// <returns>The column.</returns>
        public Column AutoIncrement(int seed, int step)
        {
            IsAutoIncrement = true;
            AutoIncrementSeed = seed;
            AutoIncrementStep = step;
            return this;
        }

        /// <summary>
        /// Makes this <see cref="Column"/> require a value.
        /// </summary>
        /// <returns>The column.</returns>
        public Column NotNull()
        {
            IsNullable = false;
            return this;
        }

        /// <summary>
        /// Sets the default value for this <see cref="Column"/>.
        /// </summary>
        /// <param name="value">The default value.</param>
        /// <returns>The column.</returns>
        public Column Default(object value)
        {
            DefaultValue = value;
            return this;
        }

        /// <summary>
        /// Sets the data type for this <see cref="Column"/>.
        /// </summary>
        /// <param name="type">The data type.</param>
        /// <returns>The column.</returns>
        public Column Type(Type type)
        {
            DataType = type.GetSqlType();
            return this;
        }

        /// <summary>
        /// Sets the data type for this <see cref="Column"/>.
        /// </summary>
        /// <param name="type">The data type.</param>
        /// <param name="length">The length of the data type.</param>
        /// <returns>The column.</returns>
        public Column Type(Type type, int length)
        {
            DataType = type.GetSqlType();
            Length = length;
            return this;
        }

        /// <summary>
        /// Sets the data type for this <see cref="Column"/>.
        /// </summary>
        /// <param name="type">The data type.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="precision">The precision.</param>
        /// <returns>The column.</returns>
        public Column Type(Type type, int scale, int precision)
        {
            DataType = type.GetSqlType();
            Scale = scale;
            Precision = precision;
            return this;
        }

        /// <summary>
        /// Sets the data type for this <see cref="Column"/>.
        /// </summary>
        /// <param name="type">The data type.</param>
        /// <returns>The column.</returns>
        public Column Type(SqlTypes type)
        {
            return Type(type, null);
        }

        /// <summary>
        /// Sets the data type for this <see cref="Column"/>.
        /// </summary>
        /// <param name="type">The data type.</param>
        /// <param name="length">The length of the data type.</param>
        /// <returns>The column.</returns>
        public Column Type(SqlTypes type, int? length)
        {
            DataType = type;
            Length = length;
            return this;
        }

        /// <summary>
        /// Sets the data type for this <see cref="Column"/>.
        /// </summary>
        /// <param name="type">The data type.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="precision">The precision.</param>
        /// <returns>The column.</returns>
        public Column Type(SqlTypes type, int scale, int precision)
        {
            DataType = type;
            Scale = scale;
            Precision = precision;
            return this;
        }

        /// <summary>
        /// Renames this <see cref="Column"/>.
        /// </summary>
        /// <param name="newName">The new name for the <see cref="Column"/>.</param>
        /// <returns>The column.</returns>
        public Column Rename(string newName)
        {
            NewName = newName;
            return this;
        }

        /// <summary>
        /// Makes this <see cref="Column"/> the primary key of the <see cref="Table"/>.
        /// </summary>
        /// <returns>The column.</returns>
        /// <remarks>Will make the <see cref="Column"/> not nullable.</remarks>
        public Column PrimaryKey()
        {
            Table.AddPrimaryKeyConstraint(Name);
            return this.NotNull();
        }

        /// <summary>
        /// Makes this <see cref="Column"/> the primary key of the <see cref="Table"/>.
        /// </summary>
        /// <param name="name">The name of the primary key constraint.</param>
        /// <returns>The column.</returns>
        /// <remarks>Will make the <see cref="Column"/> not nullable.</remarks>
        public Column PrimaryKey(string name)
        {
            Table.AddPrimaryKeyConstraint(name, Name);
            return this.NotNull();
        }

        /// <summary>
        /// Makes the values of this <see cref="Column"/> unique.
        /// </summary>
        /// <param name="name">The name of the unique constraint.</param>
        /// <returns>The column.</returns>
        public Column Unique(string name)
        {
            Table.AddUniqueConstraint(name, Name);
            return this;
        }

        /// <summary>
        /// Makes the values of this <see cref="Column"/> unique.
        /// </summary>
        /// <returns>The column.</returns>
        public Column Unique()
        {
            Table.AddUniqueConstraint(Name);
            return this;
        }

        /// <summary>
        /// Adds an <see cref="Index"/> for this <see cref="Column"/>.
        /// </summary>
        /// <param name="name">The name of the index.</param>
        /// <returns>The column.</returns>
        public Column Index(string name)
        {
            Table.AddIndex(name, Name);
            return this;
        }

        /// <summary>
        /// Adds an <see cref="Index"/> for this <see cref="Column"/>.
        /// </summary>
        /// <returns>The column.</returns>
        public Column Index()
        {
            Table.AddIndex(Name);
            return this;
        }

        /// <summary>
        /// Creates a foreign key reference to the specified <paramref name="columnName"/>
        /// in the specified <paramref name="tableName"/>.
        /// </summary>
        /// <param name="name">The name of the foreign key constraint.</param>
        /// <param name="tableName">The related table.</param>
        /// <param name="columnName">The related column.</param>
        /// <returns>The column.</returns>
        public Column References(string name, string tableName, string columnName)
        {
            Table.AddForeignKeyConstraint(name, Name, tableName, columnName);
            return this;
        }

        /// <summary>
        /// Creates a foreign key reference to the specified <paramref name="columnName"/>
        /// in the specified <paramref name="tableName"/>.
        /// </summary>
        /// <param name="tableName">The related table.</param>
        /// <param name="columnName">The related column.</param>
        /// <returns>The column.</returns>
        public Column References(string tableName, string columnName)
        {
            Table.AddForeignKeyConstraint(Name, tableName, columnName);
            return this;
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the <see cref="Table"/> this <see cref="Column" /> belongs to.
        /// </summary>
        /// <value>The table.</value>
        internal Table Table
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        internal string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the modifier.
        /// </summary>
        /// <value>The modifier.</value>
        internal Modifier Modifier
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Column"/> is an auto increment column.
        /// </summary>
        /// <value><c>true</c> if the value for this column are auto generated; otherwise, <c>false</c>.</value>
        internal bool IsAutoIncrement
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Column"/> is nullable.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this <see cref="Column"/> is nullable; otherwise, <c>false</c>.
        /// </value>
        internal bool IsNullable
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the default value for this <see cref="Column"/>.
        /// </summary>
        /// <value>The default value.</value>
        internal object DefaultValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the column's data type.
        /// </summary>
        /// <value>The type.</value>
        internal SqlTypes? DataType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the length of the <see cref="DataType"/>.
        /// </summary>
        /// <value>The length.</value>
        internal int? Length
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the column's new name (if it was renamed).
        /// </summary>
        /// <value>The new name.</value>
        internal string NewName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the precision for the <see cref="DataType"/>.
        /// </summary>
        /// <value>The precision.</value>
        internal int? Precision
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the scale for the <see cref="DataType"/>.
        /// </summary>
        /// <value>The scale.</value>
        internal int? Scale
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the auto increment seed.
        /// </summary>
        /// <value>The auto increment seed.</value>
        internal int? AutoIncrementSeed
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the auto increment step.
        /// </summary>
        /// <value>The auto increment step.</value>
        internal int? AutoIncrementStep
        {
            get;
            set;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Determines whether this <see cref="Column"/> belongs to the
        /// primary key of the <see cref="Table"/>.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if column belongs to the primary key; otherwise, <c>false</c>.
        /// </returns>
        internal bool BelongsToPrimaryKey()
        {
            var pk = Table.GetPrimaryKeyConstraint();
            return (pk != null && pk.ColumnNames.Contains(Name));
        }

        /// <summary>
        /// Determines whether this <see cref="Column"/> belongs to a
        /// foreign key reference.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if column belongs to a foreign key reference; otherwise, <c>false</c>.
        /// </returns>
        internal bool BelongsToForeignKey()
        {
            return GetForeignKeyConstraint() != null;
        }

        /// <summary>
        /// Gets the <see cref="ForeignKeyConstraint"/> this <see cref="Column"/>
        /// is part of.
        /// </summary>
        /// <returns>
        /// The <see cref="ForeignKeyConstraint"/> or <c>null</c> if
        /// the <see cref="Column"/> is not part of foreign key.
        /// </returns>
        internal ForeignKeyConstraint GetForeignKeyConstraint()
        {
            var q = (from ms in Table.Database.MigrationSteps
                     let c = ms as ForeignKeyConstraint
                     where c != null && c.Table == Table &&
                           c.ColumnNames.Contains(Name)
                     select c);

            return q.FirstOrDefault();
        }

        #endregion
    }
}