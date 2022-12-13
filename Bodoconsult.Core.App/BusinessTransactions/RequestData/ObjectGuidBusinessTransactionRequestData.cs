// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.Core.App.BusinessTransactions.RequestData;

/// <summary>
/// The request data for an business transaction request asking for a certain object GUID
/// </summary>
public class ObjectGuidBusinessTransactionRequestData : BaseBusinessTransactionRequestData
{
    /// <summary>
    /// The GUID of the requested object
    /// </summary>
    public Guid ObjectGuid { get; set; }

}