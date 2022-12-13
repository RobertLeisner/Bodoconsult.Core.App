// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Diagnostics.Tracing;

namespace Bodoconsult.Core.App.Interfaces
{
    /// <summary>
    /// Interface for event source provider used in <see cref="IAppEventSource"/>
    /// </summary>
    public interface IEventSourceProvider
    {

        /// <summary>
        /// Add <see cref="EventCounter"/> to the event source
        /// </summary>
        /// <param name="eventSource">Current event source</param>
        void AddEventCounters(EventSource eventSource);

        /// <summary>
        /// Add <see cref="IncrementingEventCounter"/> to the event source
        /// </summary>
        /// <param name="eventSource">Current event source</param>
        void AddIncrementingEventCounters(EventSource eventSource);

        /// <summary>
        /// Add e<see cref="PollingCounter"/> to the event source
        /// </summary>
        /// <param name="eventSource">Current event source</param>
        void AddPollingCounters(EventSource eventSource);

        /// <summary>
        /// Add <see cref="IncrementingPollingCounter"/> to the event source
        /// </summary>
        /// <param name="eventSource">Current event source</param>
        void AddIncrementingPollingCounters(EventSource eventSource);


    }
}
