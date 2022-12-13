// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Diagnostics.Tracing;
using Bodoconsult.Core.App.Interfaces;

namespace Bodoconsult.Core.App.EventCounters
{
    [EventSource(Name = "App-APM")]
    public sealed class AppApmEventSource : EventSource, IAppEventSource 
    {

        public static AppApmEventSource Log = new AppApmEventSource();


        /// <summary>
        /// Add an event source provider to the app event source
        /// </summary>
        /// <param name="provider">Event source provider to add</param>
        public void AddProvider(IEventSourceProvider provider)
        {

            provider.AddEventCounters(this);

            provider.AddIncrementingEventCounters(this);

            provider.AddPollingCounters(this);

            provider.AddIncrementingPollingCounters(this);

        }

        /// <summary>
        /// Event counters to be loaded in the app event source
        /// </summary>
        public Dictionary<string, EventCounter> EventCounters { get; } = new();

        /// <summary>
        /// Incrementing event counters to be loaded in the app event source
        /// </summary>
        public Dictionary<string, IncrementingEventCounter> IncrementingEventCounters { get; } = new();

        /// <summary>
        /// Polling counters to be loaded in the app event source
        /// </summary>
        public Dictionary<string, PollingCounter> PollingCounters { get; } = new();

        /// <summary>
        /// Incrementing polling counters to be loaded in the app event source
        /// </summary>
        public Dictionary<string, IncrementingPollingCounter> IncrementingPollingCounters { get; } = new();

        /// <summary>
        /// Report a value as metric value to an <see cref="EventCounter"/>
        /// </summary>
        /// <param name="name">Name of the counter</param>
        /// <param name="value">Value to report</param>
        public void ReportMetric(string name, float value)
        {
            if (!EventCounters.TryGetValue(name, out var counterInstance))
            {
                counterInstance = new EventCounter(name, this);
                EventCounters.Add(name, counterInstance);
            }
            counterInstance?.WriteMetric(value);
        }

        /// <summary>
        /// Report a value as increment to an <see cref="IncrementingEventCounter"/>
        /// </summary>
        /// <param name="name">Name of the counter</param>
        /// <param name="value">Value to report as increment</param>
        public void ReportIncrement(string name, float value)
        {
            if (!IncrementingEventCounters.TryGetValue(name, out var counterInstance))
            {
                counterInstance = new IncrementingEventCounter(name, this);
                IncrementingEventCounters.Add(name, counterInstance);
            }
            counterInstance?.Increment(value);
        }

        /// <summary>
        /// Report a value of 1 as increment to an <see cref="IncrementingEventCounter"/>
        /// </summary>
        /// <param name="name">Name of the counter</param>
        public void ReportIncrement(string name)
        {
            if (!IncrementingEventCounters.TryGetValue(name, out var counterInstance))
            {
                counterInstance = new IncrementingEventCounter(name, this);
                IncrementingEventCounters.Add(name, counterInstance);
            }
            counterInstance?.Increment();
        }


    }

}
