// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.Core.App.BusinessTransactions.RequestData;

/// <summary>
/// The request data for an business transaction request asking for a certain object ID
/// </summary>
public class ObjectIdBusinessTransactionRequestData : BaseBusinessTransactionRequestData
{
    /// <summary>
    /// The ID of the requested object
    /// </summary>
    public int ObjectId { get; set; }

}