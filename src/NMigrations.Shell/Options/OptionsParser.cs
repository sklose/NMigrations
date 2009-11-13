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
using System.ComponentModel;
using System.Linq;

namespace NMigrations.Shell.Options
{
    /// <summary>
    /// Parses command line parameters and returns the parsed values
    /// in form of an object of type <typeparamref name="T"/>. Each property
    /// of type <typeparamref name="T"/> represents one command line option.
    /// </summary>
    /// <typeparam name="T">Describes the command line options.</typeparam>
    internal class OptionsParser<T> where T : new()
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsParser"/> class.
        /// </summary>
        public OptionsParser()
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets a list of all available command line options defined by the
        /// generic type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">All properties of <see cref="T"/> are command line arguments.</typeparam>
        /// <returns>A list of all command line arguments.</returns>
        public Option[] GetOptions()
        {
            //
            // Find all properties of T
            //
            Type type = typeof(T);
            var properties = type.GetProperties();

            var options = new List<Option>();
            foreach (var prop in properties)
            {
                var opt = new Option()
                {
                    Name = ToCamelCase(prop.Name),
                    AllowMultiple = prop.PropertyType.IsArray,
                    Property = prop
                };

                //
                // Get type
                //
                bool isValueType = prop.PropertyType.IsValueType;
                bool isNullable = false;
                if (!isValueType)
                {
                    if (!prop.PropertyType.IsArray)
                    {
                        opt.Type = prop.PropertyType;
                    }
                    else
                    {
                        opt.Type = prop.PropertyType.GetElementType();
                    }
                }
                else
                {
                    if (!prop.PropertyType.IsGenericType)
                    {
                        opt.Type = prop.PropertyType;
                    }
                    // Special treatment for nullable types
                    else
                    {
                        isNullable = (typeof(int?).GetGenericTypeDefinition() == prop.PropertyType.GetGenericTypeDefinition());
                        if (isNullable)
                        {
                            opt.Type = Nullable.GetUnderlyingType(prop.PropertyType);
                        }
                        else
                        {
                            opt.Type = prop.PropertyType;
                        }
                    }
                }

                opt.IsRequired = isValueType && !isNullable;
                
                //
                // Check for attribute that overwrites name
                //
                var nameAttr = prop.GetCustomAttributes(typeof(OptionNameAttribute), false).Cast<OptionNameAttribute>();
                if (nameAttr.Count() == 1)
                {
                    opt.Name = nameAttr.ElementAt(0).Name;
                    opt.IsRequired = nameAttr.ElementAt(0).IsRequired;
                }

                //
                // Check for description
                //
                var descriptionAttr = prop.GetCustomAttributes(typeof(OptionDescriptionAttribute), false).Cast<OptionDescriptionAttribute>();
                if (descriptionAttr.Count() == 1)
                {
                    opt.Description = descriptionAttr.ElementAt(0).Description;
                }

                //
                // Check for a default value
                //
                var defaultAttr = prop.GetCustomAttributes(typeof(OptionDefaultAttribute), false).Cast<OptionDefaultAttribute>();
                if (defaultAttr.Count() == 1)
                {
                    opt.DefaultValue = defaultAttr.ElementAt(0).Value;
                }

                options.Add(opt);
            }

            return options.ToArray();
        }

        /// <summary>
        /// Parses the specified command line <paramref name="args"/> and returns
        /// an instance of <typeparamref name="T"/> containing the parsed values.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns>A instance of <typeparamref name="T"/> containing the values.</returns>
        /// <exception cref="FormatException">Command line is invalid.</exception>
        public T Parse(string[] args)
        {
            Option[] options = GetOptions();
            T result = new T();

            //
            // Parse all given arguments
            //
            var processed = new List<Option>();
            for (int i = 0; i < args.Length; i++)
            {
                //
                // Argument needs to introduce an option
                //
                if (!IsOptionName(args[i]))
                {
                    throw new FormatException();
                }
                
                //
                // Find option
                //
                string name = GetOptionName(args[i]);
                var option = options.FirstOrDefault(o => o.Name.ToLowerInvariant() == name.ToLowerInvariant());
                if (option == null)
                {
                    throw new FormatException();
                }

                //
                // Get value
                //
                if (option.Type == typeof(bool) /* booleans are flags */)
                {
                    option.Property.SetValue(result, true, null);
                }
                else
                {
                    i++; // take next argument
                    var converter = TypeDescriptor.GetConverter(option.Type);
                    if (converter == null || !converter.CanConvertFrom(typeof(string)))
                    {
                        throw new FormatException();
                    }

                    var value = converter.ConvertTo(args[i], option.Type);

                    // Set the value
                    if (option.AllowMultiple)
                    {
                        // Get the old array and create bigger new one
                        Array oldArray = option.Property.GetValue(result, null) as Array;
                        int oldLength = (oldArray == null ? 0 : oldArray.Length);
                        var newArray = Array.CreateInstance(option.Type, oldLength + 1);

                        // Copy old to new array
                        if (oldArray != null)
                        {
                            Array.Copy(oldArray, newArray, oldLength);                            
                        }

                        // Add new value
                        newArray.SetValue(value, oldLength);

                        // Assign new array
                        option.Property.SetValue(result, newArray, null);
                    }
                    else
                    {
                        option.Property.SetValue(result, value, null);
                    }
                }                

                processed.Add(option);
            }

            //
            // Process options with default values
            //
            foreach (var option in options.Except(processed).Where(o => o.DefaultValue != null))
            {
                option.Property.SetValue(result, option.DefaultValue, null);
                processed.Add(option);
            }
            
            //
            // Check if all required options a given
            //
            if (options.Except(processed).Any(o => o.IsRequired))
            {
                throw new FormatException();
            }

            return result;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Determines whether the specified <paramref name="arg"/> is an option name.
        /// Option names usually start with "/" or "-".
        /// </summary>
        /// <param name="arg">The argument.</param>
        /// <returns>
        /// 	<c>true</c> if arg is an option name; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool IsOptionName(string arg)
        {
            return arg.StartsWith("/");
        }

        /// <summary>
        /// Extracts the option name from the specified <paramref name="arg"/>.
        /// </summary>
        /// <param name="arg">The argument.</param>
        /// <returns>The option name.</returns>
        protected virtual string GetOptionName(string arg)
        {
            return arg.Substring(1);
        }

        /// <summary>
        /// Converts the specified <paramref name="str"/> to camel case.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>The converted string.</returns>
        protected virtual string ToCamelCase(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            else if (str.Length == 1)
                return str.ToLowerInvariant();
            else
                return str.Substring(0, 1).ToLowerInvariant() + str.Substring(1);
        }

        #endregion
    }
}