// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Microsoft.Extensions.Logging;

namespace Bodoconsult.Core.App.Logging
{
    /// <summary>
    /// Simple logger factory for Log4Net (loads settings from app.config)
    /// </summary>
    public class Log4NetLoggerFactory : ILoggerFactory
    {

        private ILogger _logger;

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            _logger = null;
        }


        public string ConfigFileName { get; set; } = "log4net.config";

        /// <summary>
        /// Creates a new <see cref="T:Microsoft.Extensions.Logging.ILogger" /> instance.
        /// </summary>
        /// <param name="categoryName">The category name for messages produced by the logger.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.Logging.ILogger" />.</returns>
        public ILogger CreateLogger(string categoryName)
        {
            return _logger ??= new Log4NetLogger(categoryName, ConfigFileName);
        }

        /// <summary>
        /// Adds an <see cref="T:Microsoft.Extensions.Logging.ILoggerProvider" /> to the logging system.
        /// </summary>
        /// <param name="provider">The <see cref="T:Microsoft.Extensions.Logging.ILoggerProvider" />.</param>
        public void AddProvider(ILoggerProvider provider)
        {
            throw new NotSupportedException();
        }
    }
}