// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.ComponentModel.DataAnnotations;
using Bodoconsult.Core.App.BusinessTransactions;
using Bodoconsult.Core.App.BusinessTransactions.RequestData;
using Bodoconsult.Core.App.Interfaces;
using Bodoconsult.Core.App.Logging;
using Bodoconsult.Core.App.Test.Helpers;
using Bodoconsult.Core.App.Test.TestData;

namespace Bodoconsult.Core.App.Test.BusinessTransactions;

[TestFixture]
internal class UnitTestTestTransactionManager
{


    [Test]
    public void TestCtor()
    {
        // Arrange 
        var logger = new AppLoggerProxy(new FakeLoggerFactory());
        var aes = TestHelper.CreateAppEventSource();

        // Act  
        var m = new BusinessTransactionManager(logger, aes);

        // Assert
        Assert.IsNotNull(m.CreateBusinessTransactionDelegates);
        Assert.That(m.CreateBusinessTransactionDelegates.Count, Is.EqualTo(0));

    }


    [Test]
    public void TestAddProvider()
    {
        // Arrange 
        var logger = new AppLoggerProxy(new FakeLoggerFactory());
        var aes = TestHelper.CreateAppEventSource();

        var m = new BusinessTransactionManager(logger, aes);
        var p = new TestTransactionProvider();

        // Act  
        m.AddProvider(p);

        // Assert
        Assert.That(m.CreateBusinessTransactionDelegates.Count, Is.EqualTo(p.CreateBusinessTransactionDelegates.Count));

    }

    [Test]
    public void TestCheckForBusinessTransactionSuccess()
    {
        // Arrange 
        var logger = new AppLoggerProxy(new FakeLoggerFactory());
        var aes = TestHelper.CreateAppEventSource();

        const int transactionId = 1000;
        var m = new BusinessTransactionManager(logger, aes);
        var p = new TestTransactionProvider();

        m.AddProvider(p);

        // Act  
        var t = m.CheckForBusinessTransaction(transactionId);

        // Assert
        Assert.IsNotNull(t);
        Assert.That(m.TransactionCache.Count, Is.EqualTo(1));

    }

    [Test]
    public void TestCheckForBusinessTransactionRepeatedSuccess()
    {
        // Arrange 
        var logger = new AppLoggerProxy(new FakeLoggerFactory());
        var aes = TestHelper.CreateAppEventSource();

        const int transactionId = 1000;
        var m = new BusinessTransactionManager(logger, aes);
        var p = new TestTransactionProvider();

        m.AddProvider(p);

        // Act  
        var t = m.CheckForBusinessTransaction(transactionId);
        var t2 = m.CheckForBusinessTransaction(transactionId);

        // Assert
        Assert.IsNotNull(t);
        Assert.That(m.TransactionCache.Count, Is.EqualTo(1));
        Assert.That(t2, Is.EqualTo(t));
    }

    [Test]
    public void TestCheckForBusinessTransactionNoSuccess()
    {
        // Arrange 
        var logger = new AppLoggerProxy(new FakeLoggerFactory());
        var aes = TestHelper.CreateAppEventSource();

        const int transactionId = 1000;
        var m = new BusinessTransactionManager(logger, aes);
        var p = new TestTransactionProviderNoDelegate();

        m.AddProvider(p);

        // Act and assert
        Assert.Throws<ArgumentException>(() =>
        {
            var t = m.CheckForBusinessTransaction(transactionId);
        });

    }

    [Test]
    public void TestRunBusinessTransactionSuccess()
    {
        // Arrange 
        var logger = new AppLoggerProxy(new FakeLoggerFactory());
        var aes = TestHelper.CreateAppEventSource();

        const int transactionId = 1000;
        var m = new BusinessTransactionManager(logger, aes);
        var p = new TestTransactionProvider();

        m.AddProvider(p);

        IBusinessTransactionRequestData requestData = new EmptyBusinessTransactionRequestData();

        // Act  
        var t = m.RunBusinessTransaction(transactionId, requestData);

        // Assert
        Assert.IsNotNull(t);
        Assert.That(m.TransactionCache, Has.Count.EqualTo(1));
        Assert.That(t.RequestData, Is.EqualTo(requestData));
    }

}