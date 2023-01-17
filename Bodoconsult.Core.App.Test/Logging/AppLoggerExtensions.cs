// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Core.App.Interfaces;
using Bodoconsult.Core.App.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bodoconsult.Core.App.Test.Logging;

public static class AppLoggerExtensions
{

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

                // Log4Net
                if (loggingConfig.UseLog4NetProvider)
                {
                    builder.AddLog4Net();
                    //LoadedProviders.Add("Log4Net");
                    loggingConfig.UseLog4NetProvider = true;
                }
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
    /// Create the configured default logger factory
    /// </summary>
    /// <returns>Configured default logger</returns>
    public static ILoggerFactory GetDefaultLogger()
    {
        var loggingConfig = new LoggingConfig();

        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddDefaultLogger(loggingConfig);

        var logFactory = serviceCollection.BuildServiceProvider()
            .GetService<ILoggerFactory>();

        return logFactory;
    }

    /// <summary>
    /// Create the configured fake logger factory
    /// </summary>
    /// <param name="loggingConfig">Current logging configuration</param>
    /// <returns>Configured fake logger</returns>
    public static ILoggerFactory GetFakeLogger(LoggingConfig loggingConfig)
    {

        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddFakeLogger(loggingConfig);

        var logFactory = serviceCollection.BuildServiceProvider()
            .GetService<ILoggerFactory>();

        return logFactory;
    }

    /// <summary>
    /// Create the configured fake logger factory
    /// </summary>
    /// <returns>Configured fake logger</returns>
    public static ILoggerFactory GetFakeLogger()
    {

        var loggingConfig = new LoggingConfig();

        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddFakeLogger(loggingConfig);

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
        return new AppLoggerProxy(GetFakeLogger());
    }
}