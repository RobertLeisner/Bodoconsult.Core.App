// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Core.App.BusinessTransactions.Replies;
using Bodoconsult.Core.App.Interfaces;

namespace Bodoconsult.Core.App.Test.SampleBusinessLogic
{
    internal class SampleBusinessLogicLayer
    {

        public IBusinessTransactionReply EmptyRequest(IBusinessTransactionRequestData requestData)
        {

            return new DefaultBusinessTransactionReply()
            {
                ErrorCode = 0,
                Message = "Testmessage on success"
            };
        }



    }
}
