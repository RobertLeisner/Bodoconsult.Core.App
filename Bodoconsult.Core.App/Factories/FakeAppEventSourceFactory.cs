// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.
// License MIT

using Bodoconsult.Core.App.EventCounters;
using Bodoconsult.Core.App.Interfaces;

namespace Bodoconsult.Core.App.Factories;

/// <summary>
/// Factory for a fake implementation of <see cref="IAppEventSource"/>
/// </summary>
public class FakeAppEventSourceFactory : IAppEventSourceFactory
{
    private IAppEventSource _appApmEventSource;

    /// <summary>
    /// Create an instance of a <see cref="FakeAppEventSource"/>
    /// </summary>
    /// <returns><see cref="FakeAppEventSource"/> instance</returns>
    public IAppEventSource CreateInstance()
    {
        if (_appApmEventSource != null)
        {
            return _appApmEventSource;
        }

        _appApmEventSource = new FakeAppEventSource();

        return _appApmEventSource;
    }
}