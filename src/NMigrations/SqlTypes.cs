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

namespace NMigrations
{
    /// <summary>
    /// Enumerates all known SQL data types.
    /// </summary>
    public enum SqlTypes
    {
        /// <summary>
        /// A globally unique identifier.
        /// </summary>
        Guid,

        /// <summary>
        /// A 8 bit integer value.
        /// </summary>
        TinyInt,

        /// <summary>
        /// A 16 bit integer value.
        /// </summary>
        SmallInt,

        /// <summary>
        /// A 32 bit integer value.
        /// </summary>
        Int,

        /// <summary>
        /// A 64 bit integer value.
        /// </summary>
        BigInt,

        /// <summary>
        /// A 32 bit floating point number.
        /// </summary>
        Single,

        /// <summary>
        /// A 64 bit floating point number.
        /// </summary>
        Double,

        /// <summary>
        /// A floating point number with fixed precision/scale.
        /// </summary>
        Decimal,

        /// <summary>
        /// A currency value.
        /// </summary>
        Currency,

        /// <summary>
        /// A boolean value.
        /// </summary>
        Boolean,

        /// <summary>
        /// An ANSI string of fixed length.
        /// </summary>
        Char,

        /// <summary>
        /// An ANSI string of variable length.
        /// </summary>
        VarChar,

        /// <summary>
        /// An ANSI string of maximum length.
        /// </summary>
        VarCharMax,

        /// <summary>
        /// A unicode string of fixed length.
        /// </summary>
        NChar,

        /// <summary>
        /// A unicode string of variable length.
        /// </summary>
        NVarChar,

        /// <summary>
        /// A unicode string of maximum length.
        /// </summary>
        NVarCharMax,

        /// <summary>
        /// A ANSI text blob.
        /// </summary>
        Text,

        /// <summary>
        /// A unicode text blob.
        /// </summary>
        NText,

        /// <summary>
        /// XML data.
        /// </summary>
        Xml,

        /// <summary>
        /// A date value.
        /// </summary>
        Date,

        /// <summary>
        /// A time value.
        /// </summary>
        Time,

        /// <summary>
        /// A date and time value.
        /// </summary>
        DateTime,

        /// <summary>
        /// A timestamp.
        /// </summary>
        TimeStamp,

        /// <summary>
        /// A time offset.
        /// </summary>
        TimeSpan,

        /// <summary>
        /// Binary data of fixed length.
        /// </summary>
        Binary,

        /// <summary>
        /// Binary data of variable length.
        /// </summary>
        VarBinary,

        /// <summary>
        /// Binary data of maximum length.
        /// </summary>
        VarBinaryMax
    }
}