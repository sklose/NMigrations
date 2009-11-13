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

namespace NMigrations
{
    /// <summary>
    /// Supplies metadata for a <see cref="IMigration"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class MigrationAttribute : Attribute
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MigrationAttribute"/> class.
        /// </summary>
        /// <param name="version">The version of the migration.</param>
        public MigrationAttribute(long version)
        {
            Version = version;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MigrationAttribute"/> class.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <param name="hour">The hour.</param>
        /// <param name="minute">The minute.</param>
        /// <param name="second">The second.</param>
        public MigrationAttribute(int year, int month, int day, int hour, int minute, int second)
        {
            string timestamp = year.ToString("0000") + month.ToString("00") + day.ToString("00") +
                               hour.ToString("00") + minute.ToString("00") + second.ToString("00");

            Version = long.Parse(timestamp);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public long Version
        {
            get;
            set;
        }

        #endregion
    }
}
