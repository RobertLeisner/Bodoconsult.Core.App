// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Core.App.BusinessTransactions;
using Bodoconsult.Core.App.EventCounters;
using Bodoconsult.Core.App.Logging;

namespace Bodoconsult.Core.App.Test.EventSources;

[TestFixture]
internal class UnitTestAppApmEventSource
{

    [Test]
    public void TestCtor()
    {
        // Arrange 
        var logger = new AppLoggerProxy(new FakeLoggerFactory());

        // Act  
        var aes = new AppApmEventSource(logger);

        // Assert
        Assert.That(aes.EventCounters, Is.Not.Null);
        Assert.That(aes.IncrementingEventCounters, Is.Not.Null);
        Assert.That(aes.PollingCounters, Is.Not.Null);
        Assert.That(aes.IncrementingPollingCounters, Is.Not.Null);

        Assert.That(aes.EventCounters, Has.Count.EqualTo(0));
        Assert.That(aes.IncrementingEventCounters, Has.Count.EqualTo(0));
        Assert.That(aes.PollingCounters, Has.Count.EqualTo(0));
        Assert.That(aes.IncrementingPollingCounters, Has.Count.EqualTo(0));
    }



    [Test]
    public void TestAddProvider()
    {
        // Arrange 
        var logger = new AppLoggerProxy(new FakeLoggerFactory());

        var aes = new AppApmEventSource(logger);

        // Act  
        aes.AddProvider(new BusinessTransactionEventSourceProvider());

        // Assert
        Assert.That(aes.EventCounters, Has.Count.EqualTo(1));
        Assert.That(aes.IncrementingEventCounters, Has.Count.EqualTo(1));
        Assert.That(aes.PollingCounters, Has.Count.EqualTo(0));
        Assert.That(aes.IncrementingPollingCounters, Has.Count.EqualTo(0));

    }

    [Test]
    public void TestGetMetricEventCounter()
    {
        // Arrange 
        const string ecName = BusinessTransactionEventSourceProvider.BtmRunBusinessTransactionDuration;

        var logger = new AppLoggerProxy(new FakeLoggerFactory());

        var aes = new AppApmEventSource(logger);
        aes.AddProvider(new BusinessTransactionEventSourceProvider());


        // Act  
        var ec = aes.GetMetricEventCounter(ecName);

        // Assert
        Assert.IsNotNull(ec);
        Assert.That(ec.Name, Is.EqualTo(ecName));

    }

    [Test]
    public void TestGetIncrementEventCounter()
    {
        // Arrange 
        const string ecName = BusinessTransactionEventSourceProvider.BtmRunBusinessTransactionSuccess;

        var logger = new AppLoggerProxy(new FakeLoggerFactory());

        var aes = new AppApmEventSource(logger);
        aes.AddProvider(new BusinessTransactionEventSourceProvider());

        // Act  
        var ec = aes.GetIncrementEventCounter(ecName);

        // Assert
        Assert.IsNotNull(ec);
        Assert.That(ec.Name, Is.EqualTo(ecName));

    }


}