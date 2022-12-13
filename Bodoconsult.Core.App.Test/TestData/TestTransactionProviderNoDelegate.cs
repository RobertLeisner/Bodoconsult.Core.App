// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using Bodoconsult.Core.App.BusinessTransactions;
using Bodoconsult.Core.App.Delegates;
using Bodoconsult.Core.App.Interfaces;
using Bodoconsult.Core.App.Test.SampleBusinessLogic;

namespace Bodoconsult.Core.App.Test.TestData;

internal class TestTransactionProviderNoDelegate : IBusinessTransactionProvider
{
    /// <summary>
    /// A dictionary containing delegates for creating business transactions.
    /// The key of the dictionary is the int tarnsaction ID
    /// </summary>
    public Dictionary<int, CreateBusinessTransactionDelegate> CreateBusinessTransactionDelegates { get; } = new();

    public SampleBusinessLogicLayer SampleBusinessLogic { get; }


    public TestTransactionProviderNoDelegate()
    {
        SampleBusinessLogic = new SampleBusinessLogicLayer();

        CreateBusinessTransactionDelegates.Add(1000, CreateTnr1000);


    }

    private BusinessTransaction CreateTnr1000()
    {
        return new BusinessTransaction
        {
            Id = 1000,
            Name = "Testtransaction",
        };
    }
}