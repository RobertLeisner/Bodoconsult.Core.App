// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Diagnostics.Tracing;
using Bodoconsult.Core.App.Interfaces;

namespace Bodoconsult.Core.App.EventCounters;

/// <summary>
///  Fake implementation of <see cref="IAppEventSource"/> mainly for unit testing
/// </summary>
public class FakeAppEventSource: IAppEventSource
{
    /// <summary>
    /// Add an event source provider to the app event source
    /// </summary>
    /// <param name="provider">Event source provider to add</param>
    public void AddProvider(IEventSourceProvider provider)
    {
        // Do nothing
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
        // Do nothing
    }

    /// <summary>
    /// Report a value of 1 as increment to an <see cref="IncrementingEventCounter"/>
    /// </summary>
    /// <param name="name">Name of the counter</param>
    public void ReportIncrement(string name)
    {
        // Do nothing
    }

    /// <summary>
    /// Report a value as increment to an <see cref="IncrementingEventCounter"/>
    /// </summary>
    /// <param name="name">Name of the counter</param>
    /// <param name="value">Value to report as increment</param>
    public void ReportIncrement(string name, float value)
    {
        // Do nothing
    }

    /// <summary>
    /// Get an <see cref="EventCounter"/> instance by its name
    /// </summary>
    /// <param name="name">Name of the requested instance</param>
    /// <returns><see cref="EventCounter"/> instance or null</returns>
    public EventCounter GetMetricEventCounter(string name)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get an <see cref="IAppEventSource.IncrementingEventCounters"/> instance by its name
    /// </summary>
    /// <param name="name">Name of the requested instance</param>
    /// <returns><see cref="EventCounter"/> instance or null</returns>
    public IncrementingEventCounter GetIncrementEventCounter(string name)
    {
        throw new NotImplementedException();
    }
}