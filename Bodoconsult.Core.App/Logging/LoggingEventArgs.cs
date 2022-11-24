// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.Core.App.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public class LoggingEventArgs
    {
        /// <summary>
        /// Default ctor
        /// </summary>
        /// <param name="logData">Current log data to process</param>
        public LoggingEventArgs(LogData logData)
        {
            LogData = logData;
        }

        /// <summary>
        /// Current logdata to process
        /// </summary>
        public LogData LogData { get;  }
    }
}
