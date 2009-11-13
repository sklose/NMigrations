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

using System.Linq;

namespace NMigrations.Core
{
    /// <summary>
    /// Contains extension methods for the <see cref="IMigration"/> interface.
    /// </summary>
    public static class IMigrationExtensions
    {
        /// <summary>
        /// Gets the version of the specified <paramref name="migration"/>.
        /// </summary>
        /// <param name="migration">The migration.</param>
        /// <returns>The version.</returns>
        public static long GetVersion(this IMigration migration)
        {
            var attribute =  migration.GetType().GetCustomAttributes(typeof(MigrationAttribute), false).First() as MigrationAttribute;
            return attribute.Version;
        }

        /// <summary>
        /// Gets the name of the specified <paramref name="migration"/> which is
        /// the class' name without the 'Migration' suffix.
        /// </summary>
        /// <param name="migration">The migration.</param>
        /// <returns>The migration's name.</returns>
        public static string GetName(this IMigration migration)
        {
            string name = migration.GetType().Name;
            if (name.EndsWith("Migration")) name = name.Substring(0, name.Length - "Migration".Length);
            return name;
        }
    }
}