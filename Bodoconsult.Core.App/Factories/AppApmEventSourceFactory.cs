﻿// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Core.App.BusinessTransactions;
using Bodoconsult.Core.App.EventCounters;
using Bodoconsult.Core.App.Interfaces;

namespace Bodoconsult.Core.App.Factories;

/// <summary>
/// Factory for <see cref="AppApmEventSource"/> instances
/// </summary>
public class AppApmEventSourceFactory : IAppEventSourceFactory
{

    private AppApmEventSource _appApmEventSource;

    private readonly IAppLoggerProxy _appLogger;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appLogger">Current app logger</param>
    public AppApmEventSourceFactory(IAppLoggerProxy appLogger)
    {
        _appLogger = appLogger;
    }


    /// <summary>
    /// Create a singleton instance of <see cref="AppApmEventSource"/>
    /// </summary>
    /// <returns>A singelton instance of <see cref="AppApmEventSource"/></returns>
    public IAppEventSource CreateInstance()
    {
        if (_appApmEventSource != null)
        {
            return _appApmEventSource;
        }

        _appApmEventSource = new AppApmEventSource(_appLogger);
        _appApmEventSource.AddProvider(new BusinessTransactionEventSourceProvider());

        return _appApmEventSource;
    }


}