// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Core.App.EventCounters;
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
        void AddEventCounters(AppApmEventSource eventSource);

        /// <summary>
        /// Add <see cref="IncrementingEventCounter"/> to the event source
        /// </summary>
        /// <param name="eventSource">Current event source</param>
        void AddIncrementingEventCounters(AppApmEventSource eventSource);

        /// <summary>
        /// Add e<see cref="PollingCounter"/> to the event source
        /// </summary>
        /// <param name="eventSource">Current event source</param>
        void AddPollingCounters(AppApmEventSource eventSource);

        /// <summary>
        /// Add <see cref="IncrementingPollingCounter"/> to the event source
        /// </summary>
        /// <param name="eventSource">Current event source</param>
        void AddIncrementingPollingCounters(AppApmEventSource eventSource);


    }
}
