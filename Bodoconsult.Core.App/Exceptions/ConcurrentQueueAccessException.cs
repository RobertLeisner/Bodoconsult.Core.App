// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.Core.App.Exceptions
{
    /// <summary>
    /// Errors during accessing concurrent queues
    /// </summary>
    public class ConcurrentQueueAccessException : Exception
    {
        /// <summary>
        /// Default ctor
        /// </summary>
        /// <param name="message"></param>
        public ConcurrentQueueAccessException(string message) : base(message)
        {

        }
    }
}