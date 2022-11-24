// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Microsoft.Extensions.Logging;

namespace Bodoconsult.Core.App.Test.Logging
{
    /// <summary>
    /// Base class for logger tests
    /// </summary>
    internal class BaseFakeLoggerTests
    {
        /// <summary>
        /// Current logger
        /// </summary>
        protected ILogger logger;


        /// <summary>
        /// List for all logged messages
        /// </summary>
        protected IList<string> LoggedMessages = new List<string>();

        /// <summary>
        /// Fakes the writing of an log entry
        /// </summary>
        /// <param name="message">Message to log</param>
        protected void FakeLogDelegate(string message)
        {
            LoggedMessages.Add(message);
        }
    }
}