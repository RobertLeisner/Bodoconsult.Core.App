// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Core.App.BusinessTransactions;
using Bodoconsult.Core.App.Interfaces;

namespace Bodoconsult.Core.App.Delegates
{
    /// <summary>
    /// A delegate for "status message" method calls getting a string a input parameter
    /// </summary>
    /// <param name="message">Input string</param>
    public delegate void StatusMessageDelegate(string message);

    /// <summary>
    /// Delegate for a method creating a business transaction
    /// </summary>
    /// <returns>The requested business transaction</returns>
    public delegate BusinessTransaction CreateBusinessTransactionDelegate();


    /// <summary>
    /// Run a business transaction
    /// </summary>
    /// <param name="requestData">Request data</param>
    /// <returns>A buiness action reply</returns>

    public delegate IBusinessTransactionReply RunBusinessTransactionDelegate(IBusinessTransactionRequestData requestData);
}



