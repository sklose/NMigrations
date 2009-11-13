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
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace NMigrations.Sql
{
    /// <summary>
    /// Factory class for SQL providers.
    /// </summary>
    public class SqlProviderFactory
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlProviderFactory"/> class.
        /// </summary>
        public SqlProviderFactory()
        {
            Providers = new Dictionary<string, Type>();
            RegisterProviders(GetType().Assembly);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Registers all implementations of <see cref="ISqlProvider"/> that
        /// are available in the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public void RegisterProviders(Assembly assembly)
        {
            //
            // Query for all applicable types
            //
            var q = from t in assembly.GetTypes()
                    where IsProvider(t)
                    select t;

            //
            // Fill dictionary: provider name -> type
            //
            foreach (Type type in q)
            {
                RegisterProvider(type);
            }
        }

        /// <summary>
        /// Registers a new provider of the specified <paramref name="providerType"/>.
        /// </summary>
        /// <param name="providerType">Type of the provider.</param>
        public void RegisterProvider(Type providerType)
        {
            if (IsProvider(providerType))
            {
                foreach (var attr in providerType.GetCustomAttributes(typeof(SqlProviderAttribute), false).Cast<SqlProviderAttribute>())
                {
                    if (!Providers.ContainsKey(attr.ProviderInvariantName))
                    {
                        Providers.Add(attr.ProviderInvariantName, providerType);
                    }
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a new instance of the <see cref="ISqlProvider"/> for the
        /// specified <paramref name="providerName"/>.
        /// </summary>
        /// <param name="providerName">The invariant provider name (e.g. "System.Data.SqlClient").</param>
        /// <returns>The SQL provider.</returns>
        public ISqlProvider GetProvider(string providerInvariantName)
        {
            if (Providers.ContainsKey(providerInvariantName))
            {
                return Activator.CreateInstance(Providers[providerInvariantName]) as ISqlProvider;
            }

            return null;
        }

        #endregion

        #region Protected Properties

        /// <summary>
        /// Gets or sets the registered providers.
        /// </summary>
        /// <value>The providers.</value>
        protected virtual Dictionary<string, Type> Providers
        {
            get;
            set;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Determines whether the specified type is a provider.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// 	<c>true</c> if the specified type is provider; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool IsProvider(Type type)
        {
            return /* must implement ISqlProvider */
                   type.GetInterfaces().Contains(typeof(ISqlProvider)) &&

                  /* must be instantiatable */
                  !type.IsAbstract &&

                  /* must have a default constructor */
                  type.GetConstructors().Any(ctor => ctor.IsPublic && ctor.GetParameters().Length == 0);
        }

        #endregion
    }
}
