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
using System.Reflection;

namespace NMigrations
{
    /// <summary>
    /// Contains extension methods for <see cref="Object"/> class.
    /// </summary>
    internal static class ObjectExtensions
    {
        /// <summary>
        /// Converts an <see cref="Object"/> to a <see cref="Dictionary"/> by
        /// treating all properties as key/value pairs.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The dictionary.</returns>
        public static Dictionary<string, object> ToDictionary(this object obj)
        {
            var result = new Dictionary<string, object>();
            var properties = obj.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.Public);
            foreach (var prop in properties)
            {
                // Ignore inherited items
                if (prop.DeclaringType != obj.GetType()) continue;

                try
                {
                    result.Add(prop.Name, prop.GetValue(obj, null));
                }
                catch { /* ignore */ }
            }

            return result;
        }
    }
}