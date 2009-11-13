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

namespace NMigrations.Sql
{
    /// <summary>
    /// Containts extensions methods for the <see cref="Type"/> class.
    /// </summary>
    internal static class TypeExtensions
    {
        /// <summary>
        /// Gets the corresponding SQL type for the specified CLR <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The CLR type.</param>
        /// <returns>The SQL type.</returns>
        public static SqlTypes GetSqlType(this Type type)
        {
            if (type == typeof(byte))
                return SqlTypes.TinyInt;
            else if (type == typeof(short))
                return SqlTypes.SmallInt;
            else if (type == typeof(ushort))
                return SqlTypes.SmallInt;
            else if (type == typeof(int))
                return SqlTypes.Int;
            else if (type == typeof(uint))
                return SqlTypes.Int;
            else if (type == typeof(long))
                return SqlTypes.BigInt;
            else if (type == typeof(ulong))
                return SqlTypes.BigInt;
            else if (type == typeof(double))
                return SqlTypes.Double;
            else if (type == typeof(float))
                return SqlTypes.Single;
            else if (type == typeof(decimal))
                return SqlTypes.Decimal;
            else if (type == typeof(char))
                return SqlTypes.NChar;
            else if (type == typeof(bool))
                return SqlTypes.Boolean;
            else if (type == typeof(Guid))
                return SqlTypes.Guid;
            else if (type == typeof(DateTime))
                return SqlTypes.DateTime;
            else if (type == typeof(string))
                return SqlTypes.NVarChar;

            throw new Exception("unsupported type");
        }
    }
}
