// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using Bodoconsult.Core.App.EventCounters;
using Bodoconsult.Core.App.Interfaces;

namespace Bodoconsult.Core.App.Test.Helpers
{
    internal static class TestHelper
    {
        /// <summary>
        /// Create a <see cref="IAppEventSource"/> instance
        /// </summary>
        /// <returns><see cref="IAppEventSource"/> instance based on <see cref="AppApmEventSource"/></returns>
        internal static IAppEventSource CreateAppEventSource()
        {
            var aes = new AppApmEventSource();

            return aes;
        }

    }
}
