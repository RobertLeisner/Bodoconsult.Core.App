// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.Core.App.Interfaces
{
    /// <summary>
    /// Interface for a central app date service providing current date in a testable manner
    /// </summary>
    public interface IAppDateService
    {

        /// <summary>
        /// Deliver the current date the app is running on
        /// </summary>
        DateTime Now { get; }
    }
}
