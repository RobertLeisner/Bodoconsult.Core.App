// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.
// Licence MIT

namespace Bodoconsult.Core.App.Interfaces;

/// <summary>
/// Interface for creating <see cref="IExceptionReplyBuilder"/> instances
/// </summary>
public interface IExceptionReplyBuilderFactory
{

    /// <summary>
    /// Create a new <see cref="IExceptionReplyBuilder"/> instance
    /// </summary>
    /// <param name="defaultErrorCode">Default error code</param>
    /// <returns>Instance of <see cref="IExceptionReplyBuilder"/></returns>
    IExceptionReplyBuilder CreateInstance(int defaultErrorCode);

}