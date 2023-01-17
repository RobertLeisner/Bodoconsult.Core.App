// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Bodoconsult.Core.App.Helpers;
using Bodoconsult.Core.App.Interfaces;
using Microsoft.Extensions.Logging;

namespace Bodoconsult.Core.App.Logging;

/// <summary>
/// Current implementation of <see cref="IAppLoggerProxy"/>.
/// Collect logging messages and write them in an separate job
/// </summary>
public class AppLoggerProxy : IAppLoggerProxy
{

    private IWatchDog _loggerWatchDog;

    private LogDataFactory _logDataFactory;

    public ILoggerFactory LoggerFactory { get; }

    private readonly ConcurrentQueue<LogData> _logMessages = new ConcurrentQueue<LogData>();


    //private readonly IDictionary<string, ILogger> _loggers = new Dictionary<string, ILogger>();

    private event EventHandler<LoggingEventArgs> OnLogMessage;

    private readonly ILogger _logger;

    /// <summary>
    /// Delay time in milliseconds for accessing concurrent queues
    /// </summary>
    public static int DelayTimeQueueAccess { get; set; } = 15;

    /// <summary>
    /// Default ctor
    /// </summary>
    public AppLoggerProxy(ILoggerFactory logger)
    {
        LoggerFactory = logger ?? throw new ArgumentNullException(nameof(logger));
        _logger = logger.CreateLogger("Default");
        BaseCtor();

        _logDataFactory = new LogDataFactory();
        _logDataFactory.AllocateBufferPool(50);
    }


    private void BaseCtor()
    {
        OnLogMessage += OnOnLogMessage;

        WatchDogRunnerDelegate runner = CheckQueue;

        _loggerWatchDog = new WatchDog(runner, 500, ThreadPriority.BelowNormal);

        StartLogging();
    }


    private void OnOnLogMessage(object sender, LoggingEventArgs e)
    {
        _logMessages.Enqueue(e.LogData);
    }



    /// <summary>
    /// Check the message queue for logging jobs to do.
    /// Direct ussage mainly intended for unit testing
    /// </summary>
    public async void CheckQueue()
    {

        while (_logMessages.Count > 0)
        {
            var success = _logMessages.TryDequeue(out var logData);

            if (!success || logData == null)
            {
                await Task.Delay(DelayTimeQueueAccess).ConfigureAwait(false);
                continue;
            }

            try
            {

                var fileName = FileSystemHelper.GetFileNameWithoutExtension(logData.SourceFile);

                //success = _loggers.TryGetValue(fileName, out var log);

                //if (!success)
                //{
                //    log = LoggerFactory.CreateLogger(fileName);
                //    _loggers.Add(fileName, log);
                //}

                _logger.Log(logData.LogLevel,
                    logData.EventId,
                    logData.Exception,
                    $"{logData.LogDate:yyyy.MM.dd HH:mm:ss.fffffff} - {logData.LogLevel} - {fileName}.{logData.SourceMethod}.R{logData.SourceRowNumber} - {logData.Message} {FormatArgs(logData.Args)}");

            }
            catch
            {
                Debug.Print("Logger error");
            }

            _logDataFactory.EnqueueInstance(logData);
        }
    }

    private void FireAndForget(LogData log)
    {
        OnLogMessage?.Invoke(this, new LoggingEventArgs(log));

        //// Do not run async
        // Task.Run(() => { OnLogMessage?.Invoke(this, new LoggingEventArgs(log)); });
    }

    /// <summary>Formats and writes a debug log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogDebug(0, exception, "Error while processing request from {Address}", address)</example>
    public void LogDebug(
        EventId eventId,
        Exception exception,
        string message,
        object[] args,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {

        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Debug;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;


        FireAndForget(log);
    }

    /// <summary>Formats and writes a debug log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogDebug(0, exception, "Error while processing request from {Address}", address)</example>
    public void LogDebug(
        EventId eventId,
        Exception exception,
        string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Debug;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        FireAndForget(log);

    }



    /// <summary>Formats and writes a debug log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogDebug(0, "Processing request from {Address}", address)</example>
    public void LogDebug(
        EventId eventId,
        string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Message = message;
        log.LogLevel = LogLevel.Debug;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        FireAndForget(log);
    }


    /// <summary>Formats and writes a debug log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogDebug(0, "Processing request from {Address}", address)</example>
    public void LogDebug(
        EventId eventId,
        string message,
        object[] args = null,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Message = message;
        log.LogLevel = LogLevel.Debug;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        FireAndForget(log);
    }

    /// <summary>Formats and writes a debug log message.</summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogDebug(exception, "Error while processing request from {Address}", address)</example>
    public void LogDebug(
        Exception exception,
        string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Debug;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        FireAndForget(log);
    }

    /// <summary>Formats and writes a debug log message.</summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogDebug(exception, "Error while processing request from {Address}", address)</example>
    public void LogDebug(
        Exception exception,
        string message,
        object[] args,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Debug;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        FireAndForget(log);
    }

    /// <summary>Formats and writes a debug log message.</summary>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogDebug("Processing request from {Address}", address)</example>
    public void LogDebug(string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0
    )
    {
        var log = _logDataFactory.DequeueInstance();
        log.Message = message;
        log.LogLevel = LogLevel.Debug;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        FireAndForget(log);
    }

    /// <summary>Formats and writes a debug log message.</summary>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogDebug("Processing request from {Address}", address)</example>
    public void LogDebug(string message,
        object[] args,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0
    )
    {
        var log = _logDataFactory.DequeueInstance();
        log.Message = message;
        log.LogLevel = LogLevel.Debug;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        FireAndForget(log);
    }


    /// <summary>Formats and writes a trace log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogTrace(0, exception, "Error while processing request from {Address}", address)</example>
    public void LogTrace(
        EventId eventId,
        Exception exception,
        string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Trace;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        FireAndForget(log);
    }

    /// <summary>Formats and writes a trace log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogTrace(0, exception, "Error while processing request from {Address}", address)</example>
    public void LogTrace(
        EventId eventId,
        Exception exception,
        string message,
        object[] args = null,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Trace;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        FireAndForget(log);
    }

    /// <summary>Formats and writes a trace log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogTrace(0, "Processing request from {Address}", address)</example>
    public void LogTrace(
        EventId eventId,
        string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Message = message;
        log.LogLevel = LogLevel.Trace;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        FireAndForget(log);
    }

    /// <summary>Formats and writes a trace log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogTrace(0, "Processing request from {Address}", address)</example>
    public void LogTrace(
        EventId eventId,
        string message,
        object[] args,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Message = message;
        log.LogLevel = LogLevel.Trace;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        FireAndForget(log);
    }

    /// <summary>Formats and writes a trace log message.</summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogTrace(exception, "Error while processing request from {Address}", address)</example>
    public void LogTrace(
        Exception exception,
        string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Trace;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        FireAndForget(log);
    }

    /// <summary>Formats and writes a trace log message.</summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogTrace(exception, "Error while processing request from {Address}", address)</example>
    public void LogTrace(
        Exception exception,
        string message,
        object[] args = null,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Trace;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        FireAndForget(log);
    }

    /// <summary>Formats and writes a trace log message.</summary>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogTrace("Processing request from {Address}", address)</example>
    public void LogTrace(string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Message = message;
        log.LogLevel = LogLevel.Trace;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        FireAndForget(log);
    }

    /// <summary>Formats and writes a trace log message.</summary>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogTrace("Processing request from {Address}", address)</example>
    public void LogTrace(string message,
        object[] args,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Message = message;
        log.LogLevel = LogLevel.Trace;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        FireAndForget(log);
    }

    /// <summary>Formats and writes an informational log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogInformation(0, exception, "Error while processing request from {Address}", address)</example>
    public void LogInformation(
        EventId eventId,
        Exception exception,
        string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Information;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        FireAndForget(log);
    }

    /// <summary>Formats and writes an informational log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogInformation(0, exception, "Error while processing request from {Address}", address)</example>
    public void LogInformation(
        EventId eventId,
        Exception exception,
        string message,
        object[] args,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Information;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        FireAndForget(log);
    }

    /// <summary>Formats and writes an informational log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogInformation(0, "Processing request from {Address}", address)</example>
    public void LogInformation(
        EventId eventId,
        string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Message = message;
        log.LogLevel = LogLevel.Information;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        FireAndForget(log);
    }

    /// <summary>Formats and writes an informational log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogInformation(0, "Processing request from {Address}", address)</example>
    public void LogInformation(
        EventId eventId,
        string message,
        object[] args,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Message = message;
        log.LogLevel = LogLevel.Information;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        FireAndForget(log);
    }

    /// <summary>Formats and writes an informational log message.</summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogInformation(exception, "Error while processing request from {Address}", address)</example>
    public void LogInformation(
        Exception exception,
        string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Information;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        FireAndForget(log);
    }

    /// <summary>Formats and writes an informational log message.</summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogInformation(exception, "Error while processing request from {Address}", address)</example>
    public void LogInformation(
        Exception exception,
        string message,
        object[] args,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Information;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        FireAndForget(log);
    }

    /// <summary>Formats and writes an informational log message.</summary>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogInformation("Processing request from {Address}", address)</example>
    public void LogInformation(string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Message = message;
        log.LogLevel = LogLevel.Information;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        FireAndForget(log);
    }

    /// <summary>
    /// Create a message with a special timestamp
    /// </summary>
    /// <param name="message">Message to log</param>
    /// <param name="timeStamp">Timestamp</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    public void LogInformation(string message,
        DateTime timeStamp,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Message = message;
        log.LogDate = timeStamp;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        FireAndForget(log);
    }

    /// <summary>Formats and writes an informational log message.</summary>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogInformation("Processing request from {Address}", address)</example>
    public void LogInformation(string message,
        object[] args,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Message = message;
        log.LogLevel = LogLevel.Information;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        FireAndForget(log);
    }

    /// <summary>Formats and writes a warning log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogWarning(0, exception, "Error while processing request from {Address}", address)</example>
    public void LogWarning(
        EventId eventId,
        Exception exception,
        string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Warning;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        FireAndForget(log);
    }

    /// <summary>Formats and writes a warning log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogWarning(0, exception, "Error while processing request from {Address}", address)</example>
    public void LogWarning(
        EventId eventId,
        Exception exception,
        string message,
        object[] args,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Warning;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        FireAndForget(log);
    }

    /// <summary>Formats and writes a warning log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogWarning(0, "Processing request from {Address}", address)</example>
    public void LogWarning(
        EventId eventId,
        string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Message = message;
        log.LogLevel = LogLevel.Warning;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        FireAndForget(log);
    }

    /// <summary>Formats and writes a warning log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogWarning(0, "Processing request from {Address}", address)</example>
    public void LogWarning(
        EventId eventId,
        string message,
        object[] args,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Message = message;
        log.LogLevel = LogLevel.Warning;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        FireAndForget(log);
    }

    /// <summary>Formats and writes a warning log message.</summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogWarning(exception, "Error while processing request from {Address}", address)</example>
    public void LogWarning(
        Exception exception,
        string message,
        object[] args,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Warning;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        FireAndForget(log);
    }

    /// <summary>Formats and writes a warning log message.</summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogWarning(exception, "Error while processing request from {Address}", address)</example>
    public void LogWarning(
        Exception exception,
        string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Warning;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        FireAndForget(log);
    }

    /// <summary>Formats and writes a warning log message.</summary>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogWarning("Processing request from {Address}", address)</example>
    public void LogWarning(string message,
        object[] args,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Message = message;
        log.LogLevel = LogLevel.Warning;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        FireAndForget(log);
    }


    /// <summary>Formats and writes a warning log message.</summary>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogWarning("Processing request from {Address}", address)</example>
    public void LogWarning(string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Message = message;
        log.LogLevel = LogLevel.Warning;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        FireAndForget(log);
    }

    /// <summary>Formats and writes an error log message.</summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogError("Error while processing request from 123", exception)</example>
    public void LogError(string message, Exception exception, string memberName = "", string filepath = "",
        int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Exception = exception;
        log.LogLevel = LogLevel.Error;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        FireAndForget(log);
    }

    /// <summary>Formats and writes an error log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogError(0, exception, "Error while processing request from {Address}", address)</example>
    public void LogError(
        EventId eventId,
        Exception exception,
        string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Error;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        FireAndForget(log);
    }

    /// <summary>Formats and writes an error log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogError(0, exception, "Error while processing request from {Address}", address)</example>
    public void LogError(
        EventId eventId,
        Exception exception,
        string message,
        object[] args,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Error;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        FireAndForget(log);
    }

    /// <summary>Formats and writes an error log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogError(0, "Processing request from {Address}", address)</example>
    public void LogError(
        EventId eventId,
        string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Message = message;
        log.LogLevel = LogLevel.Error;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        FireAndForget(log);
    }

    /// <summary>Formats and writes an error log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogError(0, "Processing request from {Address}", address)</example>
    public void LogError(
        EventId eventId,
        string message,
        object[] args = null,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Message = message;
        log.LogLevel = LogLevel.Error;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        FireAndForget(log);
    }

    /// <summary>Formats and writes an error log message.</summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogError(exception, "Error while processing request from {Address}", address)</example>
    public void LogError(
        Exception exception,
        string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Error;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        FireAndForget(log);
    }

    /// <summary>Formats and writes an error log message.</summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogError(exception, "Error while processing request from {Address}", address)</example>
    public void LogError(
        Exception exception,
        string message,
        object[] args = null,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Error;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        FireAndForget(log);
    }

    /// <summary>Formats and writes an error log message.</summary>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogError("Processing request from {Address}", address)</example>
    public void LogError(string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Message = message;
        log.LogLevel = LogLevel.Error;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        FireAndForget(log);
    }

    /// <summary>Formats and writes an error log message.</summary>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogError("Processing request from {Address}", address)</example>
    public void LogError(string message,
        object[] args,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Message = message;
        log.LogLevel = LogLevel.Error;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        FireAndForget(log);
    }

    /// <summary>Formats and writes a critical log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogCritical(0, exception, "Error while processing request from {Address}", address)</example>
    public void LogCritical(
        EventId eventId,
        Exception exception,
        string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Critical;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        FireAndForget(log);
    }


    /// <summary>Formats and writes a critical log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogCritical(0, exception, "Error while processing request from {Address}", address)</example>
    public void LogCritical(
        EventId eventId,
        Exception exception,
        string message,
        object[] args,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Critical;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        FireAndForget(log);
    }

    /// <summary>Formats and writes a critical log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogCritical(0, "Processing request from {Address}", address)</example>
    public void LogCritical(
        EventId eventId,
        string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Message = message;
        log.LogLevel = LogLevel.Critical;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        FireAndForget(log);
    }

    /// <summary>Formats and writes a critical log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogCritical(0, "Processing request from {Address}", address)</example>
    public void LogCritical(
        EventId eventId,
        string message,
        object[] args,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Message = message;
        log.LogLevel = LogLevel.Critical;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        FireAndForget(log);
    }

    /// <summary>Formats and writes a critical log message.</summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogCritical(exception, "Error while processing request from {Address}", address)</example>
    public void LogCritical(
        Exception exception,
        string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Critical;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        FireAndForget(log);
    }

    /// <summary>Formats and writes a critical log message.</summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogCritical(exception, "Error while processing request from {Address}", address)</example>
    public void LogCritical(
        Exception exception,
        string message,
        object[] args,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Critical;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        FireAndForget(log);
    }

    /// <summary>Formats and writes a critical log message.</summary>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogCritical("Processing request from {Address}", address)</example>
    public void LogCritical(string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Message = message;
        log.LogLevel = LogLevel.Critical;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        FireAndForget(log);
    }

    /// <summary>Formats and writes a critical log message.</summary>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogCritical("Processing request from {Address}", address)</example>
    public void LogCritical(string message,
        object[] args,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Message = message;
        log.LogLevel = LogLevel.Critical;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        FireAndForget(log);
    }

    /// <summary>
    /// Formats and writes a log message at the specified log level.
    /// </summary>
    /// <param name="logLevel">Entry will be written on this level.</param>
    /// <param name="message">Format string of the log message.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    public void Log(
        LogLevel logLevel,
        string message,
        object[] args = null,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Message = message;
        log.LogLevel = logLevel;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        FireAndForget(log);
    }

    /// <summary>
    /// Formats and writes a log message at the specified log level.
    /// </summary>
    /// <param name="logLevel">Entry will be written on this level.</param>
    /// <param name="message">Format string of the log message.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    public void Log(
        LogLevel logLevel,
        string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Message = message;
        log.LogLevel = logLevel;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        FireAndForget(log);
    }

    /// <summary>
    /// Formats and writes a log message at the specified log level.
    /// </summary>
    /// <param name="logLevel">Entry will be written on this level.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    public void Log(
        LogLevel logLevel,
        EventId eventId,
        string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Message = message;
        log.LogLevel = logLevel;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        FireAndForget(log);
    }

    /// <summary>
    /// Formats and writes a log message at the specified log level.
    /// </summary>
    /// <param name="logLevel">Entry will be written on this level.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    public void Log(
        LogLevel logLevel,
        EventId eventId,
        string message,
        object[] args = null,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Message = message;
        log.LogLevel = logLevel;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        FireAndForget(log);
    }


    /// <summary>
    /// Formats and writes a log message at the specified log level.
    /// </summary>
    /// <param name="logLevel">Entry will be written on this level.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>

    public void Log(
        LogLevel logLevel,
        Exception exception,
        string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = logLevel;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        FireAndForget(log);
    }

    /// <summary>
    /// Formats and writes a log message at the specified log level.
    /// </summary>
    /// <param name="logLevel">Entry will be written on this level.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message.</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>

    public void Log(
        LogLevel logLevel,
        Exception exception,
        string message,
        object[] args = null,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = logLevel;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        FireAndForget(log);
    }


    /// <summary>
    /// Start the logging
    /// </summary>
    public void StartLogging()
    {
        _loggerWatchDog.StartWatchDog();
    }

    /// <summary>
    /// Stop the logging
    /// </summary>
    public void StopLogging()
    {
        _loggerWatchDog.StopWatchDog();

        CheckQueue();

    }

    ///// <summary>
    ///// Formats and writes a log message at the specified log level.
    ///// </summary>
    ///// <param name="logLevel">Entry will be written on this level.</param>
    ///// <param name="eventId">The event id associated with the log.</param>
    ///// <param name="exception">The exception to log.</param>
    ///// <param name="message">Format string of the log message.</param>
    ///// <param name="args">An object array that contains zero or more objects to format.</param>
    //public  void Log(
    //  LogLevel logLevel,
    //  EventId eventId,
    //  Exception exception,
    //  string message,
    //  params object[] args)
    //{
    //    //if (logger == null)
    //    //    throw new ArgumentNullException(nameof(logger));
    //    //logger.Log<FormattedLogValues>(logLevel, eventId, new FormattedLogValues(message, args), exception, LoggerExtensions._messageFormatter);
    //}


    #region Helper methods

    /// <summary>
    /// Format args as JSON string
    /// </summary>
    /// <param name="args">Args delivered by the method caller</param>
    /// <returns>JSON formatted string</returns>
    public static string FormatArgs(object[] args)
    {

        if (args == null || args.Length == 0)
        {
            return "";
        }

        var s = new StringBuilder();

        foreach (var arg in args)
        {
            if (arg is Exception e)
            {
                s.AppendLine($"{e.GetType().Name}: {e.Message} {e.StackTrace}: " //+ Globals.JsonSerializeNice(e)
                );
            }
            else if (arg != null)
            {
                s.Append(ObjectHelper.GetObjectPropertiesAsString(arg));
            }
        }

        return s.ToString();

    }

    #endregion
    protected virtual void Dispose(bool disposing)
    {
        if (!disposing)
        {
            return;
        }
        StopLogging();
        LoggerFactory?.Dispose();

    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {

        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~AppLoggerProxy()
    {
        Dispose(false);
    }
}