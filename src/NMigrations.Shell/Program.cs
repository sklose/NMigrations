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
using System.Reflection;

using NMigrations.Core;
using NMigrations.Shell.Options;

namespace NMigrations.Shell
{
    /// <summary>
    /// The main program.
    /// </summary>
    public class Program
    {
        #region Main

        /// <summary>
        /// The program's entry point.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        public static void Main(string[] args)
        {
            //
            // Catch unhandled exceptions and terminate decently
            //
            AppDomain.CurrentDomain.UnhandledException += delegate(object sender, UnhandledExceptionEventArgs e)
            {
                Exception ex = e.ExceptionObject as Exception;
                Console.WriteLine(ex.Message);
                Environment.Exit(-1);
            };


            //
            // Print information
            //
            WriteInformation();

            //
            // Read command line arguments
            //
            CommandLineOptions arguments = null;
            try
            {
                arguments = new OptionsParser<CommandLineOptions>().Parse(args);
            }
            catch (FormatException)
            {
                WriteUsage();
                Environment.Exit(-1);
            }
            
            //
            // Get assemblies that containt the migrations
            //
            var assemblies = LoadAssemblies(arguments.Assemblies);

            //
            // Prepare the engine
            //
            var engine = new Engine();
            engine.SetConnectionString(arguments.ProviderInvariantName, arguments.ConnectionString);
            foreach (var assembly in assemblies)
            {
                engine.AddAssembly(assembly);
            }

            if (arguments.WhatIf)
            {
                engine.SetSqlProcessor(new WhatIfSqlProcessor());
                engine.SetMigrationHistoryRepository(new WhatIfMigrationHistoryRepository());
            }

            //
            // Set event handlers for status output
            //
            engine.BeforeMigration += delegate(object sender, BeforeMigrationEventArgs e)
            {
                Console.WriteLine("{0} {1}...", e.Version, (e.Direction == MigrationDirection.Up ? "UP" : "DOWN"));
            };

            engine.BeforeSql += delegate(object sender, BeforeSqlEventArgs e)
            {
                Console.WriteLine(e.Sql);
                if (arguments.Confirm)
                {
                    Console.Write("Do you want to execute this SQL statement (Y/N) ");
                    e.Cancel = (char.ToLowerInvariant(Console.ReadKey().KeyChar) == 'n');
                    Console.WriteLine();
                }
            };

            engine.AfterMigration += delegate(object sender, AfterMigrationEventArgs e)
            {
                Console.WriteLine();
                Console.WriteLine("Migration finished succcessful: {0}", e.Success);
                Console.WriteLine(new string('=', Console.WindowWidth));
                Console.WriteLine();
            };

            //
            // Run migrations
            //
            if (arguments.Version == null)
            {
                engine.Migrate();
            }
            else
            {
                engine.Migrate(arguments.Version.Value);
            }
        }

        #endregion

        #region Assemblies

        /// <summary>
        /// Loads all assemblies located at the specified <paramref name="paths"/>.
        /// </summary>
        /// <param name="paths">The paths.</param>
        /// <returns>The assemblies.</returns>
        private static IEnumerable<Assembly> LoadAssemblies(IEnumerable<string> paths)
        {
            foreach (var path in paths)
            {
                yield return Assembly.LoadFile(System.IO.Path.Combine(Environment.CurrentDirectory, path));
            }
        }

        #endregion

        #region Information

        /// <summary>
        /// Writes the copyrigt information.
        /// </summary>
        private static void WriteInformation()
        {
            var version = typeof(Program).Assembly.GetName().Version;            
            Console.WriteLine("NMigrations v{0}, (c) 2009 Sebastian Klose", version);
            Console.WriteLine();
        }

        /// <summary>
        /// Writes the list of valid command line arguments.
        /// </summary>
        private static void WriteUsage()
        {
            //
            // Define table
            //
            var options = new OptionsParser<CommandLineOptions>().GetOptions();

            //
            // Setup layout
            //
            int[] columnLengths = new int[] { 25, 10, 45 };

            //
            // Write headings
            //
            Console.Write("Argument" + new string(' ', columnLengths[0] - "Argument".Length));
            Console.Write("Required" + new string(' ', columnLengths[1] - "Required".Length));
            Console.Write("Description" + new string(' ', columnLengths[2] - "Description".Length));
            Console.Write(new string('=', Console.WindowWidth));

            //
            // Write table
            //
            foreach (var option in options.OrderByDescending(o => o.IsRequired).ThenBy(o => o.Name))
            {
                string[] row = new string[]
                {
                   option.Name,
                   option.IsRequired ? "Y" : "N",
                   option.Description
                };

                for (int i = 0; i < row.Length; i++)
                {
                    //
                    // Write max width columns
                    //
                    while (row[i].Length > columnLengths[i])
                    {
                        int x = Console.CursorLeft;
                        Console.Write(row[i].Substring(0, columnLengths[i]));
                        row[i] = row[i].Substring(columnLengths[i]);
                        Console.CursorLeft = x;
                    }

                    //
                    // Write remainder
                    //
                    Console.Write(row[i]);
                    Console.Write(new string(' ', columnLengths[i] - row[i].Length));
                }

                Console.WriteLine();
            }
        }

        #endregion
    }
}