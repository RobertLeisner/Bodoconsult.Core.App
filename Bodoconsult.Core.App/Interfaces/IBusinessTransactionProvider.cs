// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Core.App.Delegates;

namespace Bodoconsult.Core.App.Interfaces;

/// <summary>
/// Interface for a provider providing delegates for creating business transactions on demand
/// </summary>
public interface IBusinessTransactionProvider
{

    /// <summary>
    /// A dictionary containing delegates for creating business transactions.
    /// The key of the dictionary is the int tarnsaction ID
    /// </summary>
    Dictionary<int, CreateBusinessTransactionDelegate> CreateBusinessTransactionDelegates { get; }

}