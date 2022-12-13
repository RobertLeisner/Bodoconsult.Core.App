﻿// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Core.App.BusinessTransactions;

namespace Bodoconsult.Core.App.Interfaces;

/// <summary>
/// Interface for business transaction manager implementations
/// </summary>
public interface IBusinessTransactionManager
{

    /// <summary>
    /// Add the transaction delivered by the provider to an internal storage
    /// </summary>
    /// <param name="provider"></param>
    void AddProvider(IBusinessTransactionProvider provider);


    /// <summary>
    /// Check for business transaction and return it
    /// </summary>
    /// <param name="transactionId">Requested transaction ID</param>
    /// <returns>Business transaction</returns>
    BusinessTransaction CheckForBusinessTransaction(int transactionId);

    /// <summary>
    /// Run a business transaction 
    /// </summary>
    /// <param name="transactionId">ID of the requested transaction</param>
    /// <param name="requestData">Data delivered by the request</param>
    /// <returns></returns>
    IBusinessTransactionReply RunBusinessTransaction(int transactionId, IBusinessTransactionRequestData requestData);


}