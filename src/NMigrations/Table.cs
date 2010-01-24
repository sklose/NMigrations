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

namespace NMigrations
{
    /// <summary>
    /// Represents a table in the database.
    /// </summary>
    public class Table : Element
    {
        #region Internal Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Table"/> class.
        /// </summary>
        /// <param name="database">The <see cref="Database"/> this <see cref="Table"/> belongs to.</param>
        /// <param name="name">The table's name.</param>
        /// <param name="modifier">The modifier.</param>
        internal Table(Database database, string name, Modifier modifier)
            : base(name, modifier)
        {
            Database = database;
            Columns = new List<Column>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a new <see cref="Column"/> with the specified <paramref name="name"/>
        /// this this <see cref="Table"/>.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The data type.</param>
        /// <returns>The column.</returns>
        public Column AddColumn(string name, Type type)
        {
            Column c = new Column(this, name, Modifier.Add).Type(type);
            Columns.Add(c);
            return c;
        }

        /// <summary>
        /// Adds a new <see cref="Column"/> with the specified <paramref name="name"/>
        /// this this <see cref="Table"/>.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The data type.</param>
        /// <param name="length">The data type's length.</param>
        /// <returns>The column.</returns>
        public Column AddColumn(string name, Type type, int length)
        {
            Column c = new Column(this, name, Modifier.Add).Type(type, length);
            Columns.Add(c);
            return c;
        }

        /// <summary>
        /// Adds a new <see cref="Column"/> with the specified <paramref name="name"/>
        /// this this <see cref="Table"/>.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The data type.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="precision">The precision.</param>
        /// <returns>The column.</returns>
        public Column AddColumn(string name, Type type, int scale, int precision)
        {
            Column c = new Column(this, name, Modifier.Add).Type(type, scale, precision);
            Columns.Add(c);
            return c;
        }

        /// <summary>
        /// Adds a new <see cref="Column"/> with the specified <paramref name="name"/>
        /// this this <see cref="Table"/>.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The data type.</param>
        /// <returns>The column.</returns>
        public Column AddColumn(string name, SqlTypes type)
        {
            Column c = new Column(this, name, Modifier.Add).Type(type);
            Columns.Add(c);
            return c;
        }

        /// <summary>
        /// Adds a new <see cref="Column"/> with the specified <paramref name="name"/>
        /// this this <see cref="Table"/>.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The data type.</param>
        /// <param name="length">The data type's length.</param>
        /// <returns>The column.</returns>
        public Column AddColumn(string name, SqlTypes type, int length)
        {
            Column c = new Column(this, name, Modifier.Add).Type(type, length);
            Columns.Add(c);
            return c;
        }

        /// <summary>
        /// Adds a new <see cref="Column"/> with the specified <paramref name="name"/>
        /// this this <see cref="Table"/>.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The data type.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="precision">The precision.</param>
        /// <returns>The column.</returns>
        public Column AddColumn(string name, SqlTypes type, int scale, int precision)
        {
            Column c = new Column(this, name, Modifier.Add).Type(type, scale, precision);
            Columns.Add(c);
            return c;
        }

        /// <summary>
        /// Alters the <see cref="Column"/> with the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The column.</returns>
        public Column AlterColumn(string name)
        {
            Column c = new Column(this, name, Modifier.Alter);
            Columns.Add(c);
            return c;
        }

        /// <summary>
        /// Drops the <see cref="Column"/> with the specified <paramref name="name"/>
        /// from this <see cref="Table"/>.
        /// </summary>
        /// <param name="name">The name of the column to drop.</param>
        public void DropColumn(string name)
        {
            Columns.Add(new Column(this, name, Modifier.Drop));
        }

        /// <summary>
        /// Adds a primary key <see cref="Constraint"/> to this <see cref="Table"/>.
        /// </summary>
        /// <param name="columnName">The column name that makes up the primary key.</param>
        /// <returns>The primary key constraint.</returns>
        public PrimaryKeyConstraint AddPrimaryKeyConstraint(string columnName)
        {
            return AddPrimaryKeyConstraint(new string[] { columnName });
        }

        /// <summary>
        /// Adds a primary key <see cref="Constraint"/> to this <see cref="Table"/>.
        /// </summary>
        /// <param name="columnNames">The column name(s) that make(s) up the primary key.</param>
        /// <returns>The primary key constraint.</returns>
        public PrimaryKeyConstraint AddPrimaryKeyConstraint(string[] columnNames)
        {
            string name = Database.Context.SqlProvider.GetPrimaryKeyConstraintName(Name, columnNames);
            return AddPrimaryKeyConstraint(name, columnNames);
        }

        /// <summary>
        /// Adds a primary key <see cref="Constraint"/> to this <see cref="Table"/>.
        /// </summary>
        /// <param name="name">The name of the constraint.</param>
        /// <param name="columnName">The column name that makes up the primary key.</param>
        /// <returns>The primary key constraint.</returns>
        public PrimaryKeyConstraint AddPrimaryKeyConstraint(string name, string columnName)
        {
            return AddPrimaryKeyConstraint(name, new string[] { columnName });
        }

        /// <summary>
        /// Adds a primary key <see cref="Constraint"/> to this <see cref="Table"/>.
        /// </summary>
        /// <param name="name">The name of the constraint.</param>
        /// <param name="columnNames">The column name(s) that make(s) up the primary key.</param>
        /// <returns>The primary key constraint.</returns>
        public PrimaryKeyConstraint AddPrimaryKeyConstraint(string name, string[] columnNames)
        {
            var c = new PrimaryKeyConstraint(this, name, Modifier.Add)
            {
                ColumnNames = columnNames
            };

            Database.MigrationSteps.Enqueue(c);
            return c;
        }

        /// <summary>
        /// Adds a foreign key <see cref="Constraint"/> to this <see cref="Table"/>.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="relatedTableName">Name of the related table.</param>
        /// <param name="relatedColumnName">Name of the related column.</param>
        /// <returns>The foreign key constraint.</returns>
        public ForeignKeyConstraint AddForeignKeyConstraint(string columnName, string relatedTableName, string relatedColumnName)
        {
            return AddForeignKeyConstraint(new string[] { columnName }, relatedTableName, new string[] { relatedColumnName });
        }

        /// <summary>
        /// Adds a foreign key <see cref="Constraint"/> to this <see cref="Table"/>.
        /// </summary>
        /// <param name="name">The name of the constraint.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="relatedTableName">Name of the related table.</param>
        /// <param name="relatedColumnName">Name of the related column.</param>
        /// <returns>The foreign key constraint.</returns>
        public ForeignKeyConstraint AddForeignKeyConstraint(string name, string columnName, string relatedTableName, string relatedColumnName)
        {
            return AddForeignKeyConstraint(name, new string[] { columnName }, relatedTableName, new string[] { relatedColumnName });
        }

        /// <summary>
        /// Adds a foreign key <see cref="Constraint"/> to this <see cref="Table"/>.
        /// </summary>
        /// <param name="columnNames">The names of the columns.</param>
        /// <param name="relatedTableName">Name of the related table.</param>
        /// <param name="relatedColumnNames">The names of the related columns.</param>
        /// <returns>The foreign key constraint.</returns>
        public ForeignKeyConstraint AddForeignKeyConstraint(string[] columnNames, string relatedTableName, string[] relatedColumnNames)
        {
            string name = Database.Context.SqlProvider.GetForeignKeyConstraintName(Name, columnNames, relatedTableName, relatedColumnNames);
            return AddForeignKeyConstraint(name, columnNames, relatedTableName, relatedColumnNames);
        }

        /// <summary>
        /// Adds a foreign key <see cref="Constraint"/> to this <see cref="Table"/>.
        /// </summary>
        /// <param name="name">The name of the constraint.</param>
        /// <param name="columnNames">The names of the columns.</param>
        /// <param name="relatedTableName">Name of the related table.</param>
        /// <param name="relatedColumnNames">The names of the related columns.</param>
        /// <returns>The foreign key constraint.</returns>
        public ForeignKeyConstraint AddForeignKeyConstraint(string name, string[] columnNames, string relatedTableName, string[] relatedColumnNames)
        {
            var c = new ForeignKeyConstraint(this, name, Modifier.Add)
            {
                ColumnNames = columnNames,
                RelatedTableName = relatedTableName,
                RelatedColumnNames = relatedColumnNames
            };

            Database.MigrationSteps.Enqueue(c);
            return c;
        }

        /// <summary>
        /// Adds the default <see cref="Constraint"/> to this <see cref="Table"/>.
        /// </summary>
        /// <param name="name">The constraint name.</param>
        /// <param name="columnName">The name of the column that has the defaul constraint.</param>
        /// <param name="value">The default value.</param>
        /// <returns>The default constraint.</returns>
        public DefaultConstraint AddDefaultConstraint(string name, string columnName, object value)
        {
            var c = new DefaultConstraint(this, name, Modifier.Add)
            {
                ColumnName = columnName,
                Value = value
            };

            Database.MigrationSteps.Enqueue(c);
            return c;
        }

        /// <summary>
        /// Adds the default <see cref="Constraint"/> to this <see cref="Table"/>.
        /// </summary>
        /// <param name="columnName">The name of the column that has the defaul constraint.</param>
        /// <param name="value">The default value.</param>
        /// <returns>The default constraint.</returns>
        public DefaultConstraint AddDefaultConstraint(string columnName, object value)
        {
            string name = Database.Context.SqlProvider.GetDefaultConstraintName(Name, columnName);
            return AddDefaultConstraint(name, columnName, value);
        }

        /// <summary>
        /// Adds the unique <see cref="Constraint"/> to this <see cref="Table"/>.
        /// </summary>
        /// <param name="columnName">The name of the column that needs to be unique.</param>
        /// <returns>The unique constraint.</returns>
        public UniqueConstraint AddUniqueConstraint(string columnName)
        {
            return AddUniqueConstraint(new string[] { columnName });
        }

        /// <summary>
        /// Adds the unique <see cref="Constraint"/> to this <see cref="Table"/>.
        /// </summary>
        /// <param name="columnNames">The names of the columns that in conjunction need to be unique.</param>
        /// <returns>The unique constraint.</returns>
        public UniqueConstraint AddUniqueConstraint(string[] columnNames)
        {
            string name = Database.Context.SqlProvider.GetUniqueConstraintName(Name, columnNames);
            return AddUniqueConstraint(name, columnNames);
        }

        /// <summary>
        /// Adds the unique <see cref="Constraint"/> to this <see cref="Table"/>.
        /// </summary>
        /// <param name="name">The name of the constraint.</param>
        /// <param name="columnName">The name of the column needs to be unique.</param>
        /// <returns>The unique constraint.</returns>
        public UniqueConstraint AddUniqueConstraint(string name, string columnName)
        {
            return AddUniqueConstraint(name, new string[] { columnName });
        }

        /// <summary>
        /// Adds the unique <see cref="Constraint"/> to this <see cref="Table"/>.
        /// </summary>
        /// <param name="name">The name of the constraint.</param>
        /// <param name="columnNames">The names of the columns that in conjunctions need to be unique.</param>
        /// <returns>The unique constraint.</returns>
        public UniqueConstraint AddUniqueConstraint(string name, string[] columnNames)
        {
            var c = new UniqueConstraint(this, name, Modifier.Add)
            {
                ColumnNames = columnNames
            };

            Database.MigrationSteps.Enqueue(c);
            return c;
        }

        /// <summary>
        /// Drops the <see cref="Constraint"/> with the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The constraint's name.</param>
        /// <returns>The constraint.</returns>
        public Constraint DropConstraint(string name)
        {
            var c = new Constraint(this, name, Modifier.Drop);
            Database.MigrationSteps.Enqueue(c);
            return c;
        }

        /// <summary>
        /// Drops the <see cref="PrimaryKeyConstraint"/> with the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The constraint's name.</param>
        /// <returns>The constraint.</returns>
        public PrimaryKeyConstraint DropPrimaryKeyConstraint(string name)
        {
            var c = new PrimaryKeyConstraint(this, name, Modifier.Drop);
            Database.MigrationSteps.Enqueue(c);
            return c;
        }

        /// <summary>
        /// Drops the <see cref="PrimaryKeyConstraint"/>.
        /// </summary>
        /// <returns>The constraint.</returns>
        public PrimaryKeyConstraint DropPrimaryKeyConstraint()
        {
            return DropPrimaryKeyConstraint(null);
        }

        /// <summary>
        /// Drops the <see cref="ForeignKeyConstraint"/> with the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The constraint's name.</param>
        /// <returns>The constraint.</returns>
        public ForeignKeyConstraint DropForeignKeyConstraint(string name)
        {
            var c = new ForeignKeyConstraint(this, name, Modifier.Drop);
            Database.MigrationSteps.Enqueue(c);
            return c;
        }

        /// <summary>
        /// Drops the <see cref="UniqueConstraint"/> with the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The constraint's name.</param>
        /// <returns>The constraint.</returns>
        public UniqueConstraint DropUniqueConstraint(string name)
        {
            var c = new UniqueConstraint(this, name, Modifier.Drop);
            Database.MigrationSteps.Enqueue(c);
            return c;
        }

        /// <summary>
        /// Drops the <see cref="DefaultConstraint"/> with the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The constraint's name.</param>
        /// <returns>The constraint.</returns>
        public DefaultConstraint DropDefaultConstraint(string name)
        {
            var c = new DefaultConstraint(this, name, Modifier.Drop);
            Database.MigrationSteps.Enqueue(c);
            return c;
        }

        /// <summary>
        /// Drops the <see cref="DefaultConstraint"/> of the specified <paramref name="columnName"/>.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns>The constraint.</returns>
        public DefaultConstraint DropDefaultConstraintByColumnName(string columnName)
        {
            var c = new DefaultConstraint(this, null, Modifier.Drop)
            {
                ColumnName = columnName
            };
            Database.MigrationSteps.Enqueue(c);
            return c;
        }

        /// <summary>
        /// Adds a new <see cref="Index"/> to this <see cref="Table"/>.
        /// </summary>
        /// <param name="columnName">The column name.</param>
        /// <returns>The index.</returns>
        public Index AddIndex(string columnName)
        {
            return AddIndex(new string[] { columnName });
        }

        /// <summary>
        /// Adds a new <see cref="Index"/> to this <see cref="Table"/>.
        /// </summary>
        /// <param name="columnNames">The column names.</param>
        /// <returns>The index.</returns>
        public Index AddIndex(params string[] columnNames)
        {
            string name = Database.Context.SqlProvider.GetIndexName(Name, columnNames);
            return AddIndex(name, columnNames);
        }

        /// <summary>
        /// Adds a new <see cref="Index"/> to this <see cref="Table"/>.
        /// </summary>
        /// <param name="name">The name of the index.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns>The index.</returns>
        public Index AddIndex(string name, string columnName)
        {
            return AddIndex(name, new string[] { columnName });
        }

        /// <summary>
        /// Adds a new <see cref="Index"/> to this <see cref="Table"/>.
        /// </summary>
        /// <param name="name">The name of the index.</param>
        /// <param name="columnNames">The column names.</param>
        /// <returns>The index.</returns>
        public Index AddIndex(string name, string[] columnNames)
        {
            Index i = new Index(this, name, Modifier.Add)
            {
                ColumnNames = columnNames
            };

            Database.MigrationSteps.Enqueue(i);
            return i;
        }

        /// <summary>
        /// Drops the <see cref="Index"/> with the specified
        /// <paramref name="name"/> from this <see cref="Table"/>.
        /// </summary>
        /// <param name="name">The name of the index.</param>
        public void DropIndex(string name)
        {
            Database.MigrationSteps.Enqueue(new Index(this, name, Modifier.Drop));
        }

        /// <summary>
        /// Inserts the specified row into this <see cref="Table"/>.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <returns>The table.</returns>
        public Table Insert(Dictionary<string, object> row)
        {
            Database.MigrationSteps.Enqueue(new Insert(this, row));
            return this;
        }

        /// <summary>
        /// Inserts the specified row into this <see cref="Table"/>.
        /// </summary>
        /// <param name="row">The row as anonymous object (properties are key/value pairs for row).</param>
        /// <returns>The table.</returns>
        public Table Insert(object row)
        {
            Database.MigrationSteps.Enqueue(new Insert(this, row));
            return this;
        }

        /// <summary>
        /// Updates the specified rows described by the <paramref name="where"/>
        /// argument with the new values specified by the <paramref name="set"/> argument.
        /// </summary>
        /// <param name="set">The columns new values.</param>
        /// <param name="where">The update's where clause.</param>
        /// <returns>The table.</returns>
        public Table Update(Dictionary<string, object> set, Dictionary<string, object> where)
        {
            Database.MigrationSteps.Enqueue(new Update(this, set, where));
            return this;
        }

        /// <summary>
        /// Updates the specified rows described by the <paramref name="where"/>
        /// argument with the new values specified by the <paramref name="set"/> argument.
        /// </summary>
        /// <param name="set">The columns to update as an anonymous object (properties are key/value pairs for row).</param>
        /// <param name="where">The update's where clause as an anonymous object.</param>
        /// <returns>The table.</returns>
        public Table Update(object set, object where)
        {
            Database.MigrationSteps.Enqueue(new Update(this, set, where));
            return this;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Determines whether this <see cref="Table"/> has a primary key.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if a primary key exists; otherwise, <c>false</c>.
        /// </returns>
        internal bool HasPrimaryKey()
        {
            return GetPrimaryKeyConstraint() != null;
        }

        /// <summary>
        /// Determines whether the primary key of this <see cref="Table"/> consists of more than one <see cref="Column"/>.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if the primary key is compound; otherwise, <c>false</c>.
        /// </returns>
        internal bool HasCompoundPrimaryKey()
        {
            var pk = GetPrimaryKeyConstraint();
            return (pk != null && pk.ColumnNames.Length > 1);
        }


        /// <summary>
        /// Gets the <see cref="PrimaryKeyConstraint"/> of this <see cref="Table"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="PrimaryKeyConstraint"/> or <c>null</c> if
        /// the <see cref="Table"/> has no primary key.
        /// </returns>
        internal PrimaryKeyConstraint GetPrimaryKeyConstraint()
        {
            var q = (from ms in Database.MigrationSteps
                     let pk = ms as PrimaryKeyConstraint
                     where pk != null && pk.Table == this
                     select pk);

            return q.FirstOrDefault();
        }

        /// <summary>
        /// Gets all <see cref="ForeignKeyConstraint"/>s that belog thi this <see cref="Table"/>.
        /// </summary>
        /// <returns>The <see cref="ForeignKeyConstraint"/>s.</returns>
        internal IEnumerable<ForeignKeyConstraint> GetForeignKeyConstraints()
        {
            return (from ms in Database.MigrationSteps
                    let fk = ms as ForeignKeyConstraint
                    where fk != null && fk.Table == this
                    select fk);
        }

        /// <summary>
        /// Gets all <see cref="UniqueConstraints"/>s that belog thi this <see cref="Table"/>.
        /// </summary>
        /// <returns>The <see cref="UniqueConstraint"/>s.</returns>
        internal IEnumerable<UniqueConstraint> GetUniqueConstraints()
        {
            return (from ms in Database.MigrationSteps
                    let uq = ms as UniqueConstraint
                    where uq != null && uq.Table == this
                    select uq);
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the <see cref="Database"/> this <see cref="Table"/> belongs to.
        /// </summary>
        /// <value>The database.</value>
        internal Database Database
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the columns.
        /// </summary>
        /// <value>The columns.</value>
        internal List<Column> Columns
        {
            get;
            set;
        }

        #endregion
    }
}