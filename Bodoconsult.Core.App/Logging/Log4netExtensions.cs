// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Bodoconsult.Core.App.Logging;

public static class Log4NetExtensions
{
    public static ILoggerFactory AddLog4Net(this ILoggerFactory factory, string log4NetConfigFile)
    {
        if (factory == null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        using (var p = new Log4NetProvider(log4NetConfigFile))
        {
            factory.AddProvider(p);
        }

        return factory;
    }

    public static ILoggerFactory AddLog4Net(this ILoggerFactory factory)
    {
        if (factory == null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        var s = typeof(Log4NetExtensions).Assembly.Location;

        // ReSharper disable once AssignNullToNotNullAttribute
        s = Path.Combine(new FileInfo(s).DirectoryName, "log4net.config");

        using (var p = new Log4NetProvider(s))
        {
            factory.AddProvider(p);
        }

        return factory;
    }


    /// <summary>Adds a debug logger named 'Debug' to the factory.</summary>
    /// <param name="builder">The extension method argument.</param>
    public static ILoggingBuilder AddLog4Net(this ILoggingBuilder builder)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, Log4NetProvider>());
        return builder;
    }
}