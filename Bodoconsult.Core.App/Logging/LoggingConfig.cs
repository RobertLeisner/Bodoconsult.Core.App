// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.EventLog;

namespace Bodoconsult.Core.App.Logging;

/// <summary>
/// Stores logging configuration
/// </summary>

public class LoggingConfig
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public LoggingConfig()
    {
        Filters = new Dictionary<string, LogLevel>();
        MinimumLogLevel = LogLevel.Information;
        ConsoleConfigurationSettings = new SimpleConsoleFormatterOptions();
        EventLogSettings = new EventLogSettings { SourceName = "MyApp" };
    }

    /// <summary>
    /// Ctor providing an app name
    /// </summary>
    public LoggingConfig(string appName)
    {
        Filters = new Dictionary<string, LogLevel>();
        MinimumLogLevel = LogLevel.Information;
        ConsoleConfigurationSettings = new SimpleConsoleFormatterOptions();
        EventLogSettings = new EventLogSettings { SourceName = appName };
    }

    /// <summary>
    /// Minimum log level to set
    /// </summary>
    public LogLevel MinimumLogLevel { get; set; }


    /// <summary>
    /// Output filters set for logging
    /// </summary>
    public Dictionary<string, LogLevel> Filters { get; }


    /// <summary>
    /// Use the log4net rovider als logging target. Requires log4net.config in the app root folder.
    /// </summary>
    public bool UseLog4NetProvider { get; set; }

    /// <summary>
    /// Use the Debug provide to log in output window of Visual Studio
    /// </summary>
    public bool UseDebugProvider { get; set; }

    /// <summary>
    /// Use the Console provide to log in console window
    /// </summary>
    public bool UseConsoleProvider { get; set; }


    /// <summary>
    /// Console config configuration
    /// </summary>
    public SimpleConsoleFormatterOptions ConsoleConfigurationSettings { get; }

    /// <summary>
    /// Use the EventLog provide to log to Windows event log
    /// </summary>
    public bool UseEventLogProvider { get; set; }

    /// <summary>
    /// Settings for EventLog logging
    /// </summary>
    public EventLogSettings EventLogSettings { get; }

    /// <summary>
    /// Use the EventSource provider to log to Event Tracing on Windows (ETW) EventSource
    /// </summary>
    public bool UseEventSourceProvider { get; set; }
}