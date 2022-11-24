// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.Core.App.Interfaces;

/// <summary>
/// Interface for performance logging implementations
/// </summary>
public interface IPerformanceLogger
{
    /// <summary>
    /// Get important counters as formatted string
    /// </summary>
    /// <returns>String with performance counter data</returns>
    string GetCountersAsString();

}