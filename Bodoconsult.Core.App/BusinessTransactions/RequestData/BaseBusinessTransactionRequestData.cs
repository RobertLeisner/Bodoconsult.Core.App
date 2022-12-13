// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Core.App.Interfaces;

namespace Bodoconsult.Core.App.BusinessTransactions.RequestData
{
    /// <summary>
    /// Base class for <see cref="IBusinessTransactionRequestData"/> implementations
    /// </summary>
    public abstract class BaseBusinessTransactionRequestData: IBusinessTransactionRequestData
    {
        /// <summary>
        /// The ID of the requested business transaction
        /// </summary>
        public int TransactionId { get; set; }

        /// <summary>
        /// Unique GUID of the transaction
        /// </summary>
        public Guid TransactionGuid { get; } = Guid.NewGuid();

        /// <summary>
        /// Request meta data: client GUID
        /// </summary>
        public Guid MetaDataClientGuid { get; set; }

        /// <summary>
        /// Request meta data: client name
        /// </summary>
        public string MetaDataClientName { get; set; }

        /// <summary>
        /// Request meta data: IP address the request is coming from
        /// </summary>
        public string MetaDataIpAddress { get; set; }

        /// <summary>
        /// Request meta data: user name in cleartext
        /// </summary>
        public string MetaDataUserName { get; set; }

        /// <summary>
        /// Request meta data: User ID
        /// </summary>
        public int MetaDataUserId { get; set; }
    }
}
