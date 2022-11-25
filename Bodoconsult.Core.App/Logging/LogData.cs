// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Core.App.Interfaces;
using Microsoft.Extensions.Logging;

namespace Bodoconsult.Core.App.Logging
{
    /// <summary>
    /// Log data entity class
    /// </summary>
    public class LogData: IResetable
    {
        /// <summary>
        /// The timestamp the log entry was originally created
        /// </summary>
        public DateTime LogDate { get; internal set; } = DateTime.Now;


        /// <summary>
        /// Actual log level
        /// </summary>
        public LogLevel LogLevel { get; set; } = LogLevel.Information;


        /// <summary>
        /// Source file
        /// </summary>
        public string SourceFile { get; set; } = string.Empty;

        /// <summary>
        /// Source method
        /// </summary>
        public string SourceMethod { get; set; } = string.Empty;

        /// <summary>
        /// Source row number
        /// </summary>
        public int SourceRowNumber { get; set; }

        /// <summary>
        /// Message to log
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Exception to log
        /// </summary>
        public Exception? Exception { get; set; } = null;


        /// <summary>
        /// EventId to log
        /// </summary>
        public EventId EventId  { get; set; }

        /// <summary>
        /// Args delivered from the caller
        /// </summary>
        public object[]? Args { get; set; } = null;

        /// <summary>
        /// Reset the class to default values
        /// </summary>
        public void Reset()
        {
            LogDate = DateTime.Now;
            LogLevel = LogLevel.Information;
            SourceFile = string.Empty;
            SourceMethod = string.Empty;
            Exception = null;
            Args = null;
        }
    }
}