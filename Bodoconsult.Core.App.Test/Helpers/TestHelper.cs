// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using Bodoconsult.Core.App.BusinessTransactions;
using Bodoconsult.Core.App.EventCounters;
using Bodoconsult.Core.App.Interfaces;
using Bodoconsult.Core.App.Logging;

namespace Bodoconsult.Core.App.Test.Helpers;

internal static class TestHelper
{
    /// <summary>
    /// Create a <see cref="IAppEventSource"/> instance
    /// </summary>
    /// <returns><see cref="IAppEventSource"/> instance based on <see cref="AppApmEventSource"/></returns>
    internal static IAppEventSource CreateAppEventSource()
    {
        var logger = new AppLoggerProxy(new FakeLoggerFactory());

        var aes = new AppApmEventSource(logger);
        aes.AddProvider(new BusinessTransactionEventSourceProvider());

        return aes;
    }

}