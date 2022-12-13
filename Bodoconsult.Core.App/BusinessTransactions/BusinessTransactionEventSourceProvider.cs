// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Diagnostics.Tracing;
using Bodoconsult.Core.App.Interfaces;

namespace Bodoconsult.Core.App.BusinessTransactions
{
    public class BusinessTransactionEventSourceProvider: IEventSourceProvider
    {
        public const string BtmRunBusinessTransactionSuccess = "Btm.RunBusinessTransaction.Success";

        public const string BtmRunBusinessTransactionDuration = "Btm.RunBusinessTransaction.Duration";

        /// <summary>
        /// Add <see cref="EventCounter"/> to the event source
        /// </summary>
        /// <param name="eventSource">Current event source</param>
        public void AddEventCounters(EventSource eventSource)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Add <see cref="IncrementingEventCounter"/> to the event source
        /// </summary>
        /// <param name="eventSource">Current event source</param>
        public void AddIncrementingEventCounters(EventSource eventSource)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Add e<see cref="PollingCounter"/> to the event source
        /// </summary>
        /// <param name="eventSource">Current event source</param>
        public void AddPollingCounters(EventSource eventSource)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Add <see cref="IncrementingPollingCounter"/> to the event source
        /// </summary>
        /// <param name="eventSource">Current event source</param>
        public void AddIncrementingPollingCounters(EventSource eventSource)
        {
            throw new NotImplementedException();
        }
    }
}
