// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Core.App.Interfaces;

namespace Bodoconsult.Core.App.CentralServices
{
    /// <summary>
    /// Current implementation of <see cref="IAppDateService"/> based on DateTime.Now()
    /// </summary>
    public class AppDateService: IAppDateService
    {
        /// <summary>
        /// Deliver the current date the app is running on
        /// </summary>
        public DateTime Now => DateTime.Now;
    }
}
