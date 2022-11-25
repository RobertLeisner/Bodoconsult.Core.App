// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


namespace Bodoconsult.Core.App.Interfaces;

/// <summary>
/// Interface for classes which should be reusable to reduce GC pressure
/// </summary>
public interface IResetable
{

    /// <summary>
    /// Reset the class to default values
    /// </summary>
    void Reset();
}