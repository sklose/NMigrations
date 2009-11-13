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

using System.Collections.Generic;

namespace NMigrations.Sql
{
    /// <summary>
    /// Contains extension methods for the <see cref="Queue"/> class.
    /// </summary>
    internal static class QueueExtensions
    {
        /// <summary>
        /// Removes the specified <paramref name="element"/> from the <see cref="Queue"/>.
        /// </summary>
        /// <typeparam name="T">The type of the queue's elements.</typeparam>
        /// <param name="queue">The queue.</param>
        /// <param name="element">The element to remove.</param>
        public static void Remove<T>(this Queue<T> queue, T element) where T : class
        {
            Queue<T> tmp = new Queue<T>();

            //
            // Write items (except "element") to temp queue
            //
            while (queue.Count > 0)
            {
                T t = queue.Dequeue();
                if (!t.Equals(element)) tmp.Enqueue(t);
            }

            //
            // Copy back to original queue
            //
            while (tmp.Count > 0)
            {
                queue.Enqueue(tmp.Dequeue());
            }
        }
    }
}
