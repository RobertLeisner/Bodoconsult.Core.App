// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Diagnostics.Tracing;
using Bodoconsult.Core.App.EventCounters;
using Bodoconsult.Core.App.Interfaces;

namespace Bodoconsult.Core.App.BusinessTransactions
{
    /// <summary>
    /// Provider for business transaction management relevante event counters
    /// </summary>
    public class BusinessTransactionEventSourceProvider: IEventSourceProvider
    {
        public const string BtmRunBusinessTransactionSuccess = "Btm.RunBusinessTransaction.Success";

        public const string BtmRunBusinessTransactionDuration = "Btm.RunBusinessTransaction.Duration";

        /// <summary>
        /// Add <see cref="EventCounter"/> to the event source
        /// </summary>
        /// <param name="eventSource">Current event source</param>
        public void AddEventCounters(AppApmEventSource eventSource)
        {
            CreateBtRunTransactionDurationEventCounter(eventSource);
        }

        private void CreateBtRunTransactionDurationEventCounter(AppApmEventSource eventSource)
        {
            var ec = new EventCounter(BtmRunBusinessTransactionDuration, eventSource);
            ec.DisplayName = "Business transaction duration";
            ec.DisplayUnits = "ms";

            eventSource.EventCounters.Add(BtmRunBusinessTransactionDuration, ec);
        }


        /// <summary>
        /// Add <see cref="IncrementingEventCounter"/> to the event source
        /// </summary>
        /// <param name="eventSource">Current event source</param>
        public void AddIncrementingEventCounters(AppApmEventSource eventSource)
        {
            CreateRunBtSuccessIncrementEventCounter(eventSource);
        }

        private void CreateRunBtSuccessIncrementEventCounter(AppApmEventSource eventSource)
        {
            var ec = new IncrementingEventCounter(BtmRunBusinessTransactionSuccess, eventSource);
            ec.DisplayName = "Business transaction running successfully";
            ec.DisplayUnits = "runs";

            eventSource.IncrementingEventCounters.Add(BtmRunBusinessTransactionSuccess, ec);
        }

        /// <summary>
        /// Add e<see cref="PollingCounter"/> to the event source
        /// </summary>
        /// <param name="eventSource">Current event source</param>
        public void AddPollingCounters(AppApmEventSource eventSource)
        {
            // Do nothing
        }

        /// <summary>
        /// Add <see cref="IncrementingPollingCounter"/> to the event source
        /// </summary>
        /// <param name="eventSource">Current event source</param>
        public void AddIncrementingPollingCounters(AppApmEventSource eventSource)
        {
            // Do nothing
        }
    }
}
