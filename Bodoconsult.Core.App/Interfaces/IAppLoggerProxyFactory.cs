// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using Microsoft.Extensions.Logging;

namespace Bodoconsult.Core.App.Interfaces;

/// <summary>
/// Interface for creating an instance of a <see cref="IAppLoggerProxy"/> implementation
/// </summary>
public interface IAppLoggerProxyFactory
{
    /// <summary>
    /// Create an instance of a <see cref="IAppLoggerProxy"/> implementation
    /// </summary>
    /// <param name="loggerFactory">Current logger factory to use</param>
    /// <returns>An instance of a <see cref="IAppLoggerProxy"/></returns>
    IAppLoggerProxy CreateInstance(ILoggerFactory loggerFactory);
}