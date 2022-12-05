// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Diagnostics;
using Bodoconsult.Core.App.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bodoconsult.Core.App.Logging
{
    /// <summary>
    /// Helper functionality for easy usage of logging
    /// </summary>
    public static class AppLoggerExtensions
    {

        /// <summary>
        /// Add a debug logger
        /// </summary>
        /// <param name="serviceCollection">Current service collection</param>
        /// <param name="loggingConfig">Current logger configuration</param>
        public static void AddDebugLogger(this IServiceCollection serviceCollection, LoggingConfig loggingConfig)
        {
            serviceCollection.AddLogging(builder =>
                {
                    // Add minimum log level from config
                    builder.SetMinimumLevel(loggingConfig.MinimumLogLevel);

                    // Add filters from config
                    foreach (var filter in loggingConfig.Filters)
                    {
                        var key = filter.Key.ToUpperInvariant() == "Default" ? null : filter.Key;
                        builder.AddFilter(key, filter.Value);
                    }

                    // Add the providers
                    // Debug
                    builder.AddDebug();
                    //LoadedProviders.Add("Debug");
                    loggingConfig.UseDebugProvider = true;
                }
            );
        }

        /// <summary>
        /// Add a default logger
        /// </summary>
        /// <param name="serviceCollection">Current service collection</param>
        /// <param name="loggingConfig">Current logger configuration</param>

        public static void AddDefaultLogger(this IServiceCollection serviceCollection, LoggingConfig loggingConfig)
        {
            serviceCollection.AddLogging(builder =>
                {

                    // Clear all default providers
                    builder.ClearProviders();


                    // Add minimum log level from config
                    builder.SetMinimumLevel(loggingConfig.MinimumLogLevel);


                    // Add filters from config
                    foreach (var filter in loggingConfig.Filters)
                    {
                        var key = filter.Key.ToUpperInvariant() == "Default" ? null : filter.Key;
                        builder.AddFilter(key, filter.Value);
                    }

                    // Add the providers

                    //// Event source
                    //if (loggingConfig.UseEventSourceProvider)
                    //{
                    //    builder.AddEventSourceLogger();
                    //    //LoadedProviders.Add("EventSource");
                    //    loggingConfig.UseEventSourceProvider = true;
                    //}

                    // Log4Net
                    if (loggingConfig.UseLog4NetProvider)
                    {
                        builder.AddLog4Net();
                        //LoadedProviders.Add("Log4Net");
                        loggingConfig.UseLog4NetProvider = true;
                    }
#if DEBUG
                    // Debug
                    if (Debugger.IsAttached && loggingConfig.UseDebugProvider)
                    {
                        builder.AddDebug();
                        //LoadedProviders.Add("Debug");
                        loggingConfig.UseDebugProvider = true;
                    }
#endif

                    // Console
                    if (loggingConfig.UseConsoleProvider)
                    {

                        builder.AddSimpleConsole(x =>
                        {
                            x.ColorBehavior = loggingConfig.ConsoleConfigurationSettings.ColorBehavior;
                            x.IncludeScopes = loggingConfig.ConsoleConfigurationSettings.IncludeScopes;

                        });
                        //LoadedProviders.Add("Console");
                        loggingConfig.UseConsoleProvider = true;
                    }

                    // EventLog
                    if (!loggingConfig.UseEventLogProvider)
                    {
                        return;
                    }

                    builder.AddEventLog(loggingConfig.EventLogSettings);
                    //LoadedProviders.Add("EventLog");
                    loggingConfig.UseDebugProvider = true;
                }
            );
        }

        /// <summary>
        /// Add a fake logger for testing purposes
        /// </summary>
        /// <param name="serviceCollection">Current service collection</param>
        /// <param name="loggingConfig">Current logger configuration</param>

        public static void AddFakeLogger(this IServiceCollection serviceCollection, LoggingConfig loggingConfig)
        {
            // ToDo: use logger config
            serviceCollection.AddSingleton<ILoggerFactory, FakeLoggerFactory>();
        }


        /// <summary>
        /// Create the configured default logger factory
        /// </summary>
        /// <param name="loggingConfig">Current logging configuration</param>
        /// <returns>Configured default logger</returns>
        public static ILoggerFactory GetDefaultLogger(LoggingConfig loggingConfig)
        {

            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddDefaultLogger(loggingConfig);

            var logFactory = serviceCollection.BuildServiceProvider()
                .GetService<ILoggerFactory>();

            return logFactory;
        }

        /// <summary>
        /// Get a fake app logger proxy
        /// </summary>
        /// <returns></returns>
        public static IAppLoggerProxy GetFakeAppLoggerProxy()
        {
            return new AppLoggerProxy(new FakeLoggerFactory());
        }


        /// <summary>
        /// Get a default app logger proxy
        /// </summary>
        /// <returns></returns>
        public static IAppLoggerProxy GetDefaultAppLoggerProxy(LoggingConfig loggingConfig)
        {
            return new AppLoggerProxy(GetDefaultLogger(loggingConfig));
        }
    }
}
