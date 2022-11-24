// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Core.App.PerformanceLogging;

namespace Bodoconsult.Core.App.Test.PerformanceLogging;

[TestFixture]
public class UnitTestPerformanceLoggerManager
{

    private bool _delegateWasFired;

    [Test]
    public void TestCtor()
    {
        // Arrange 
        var logger = new PerformanceLogger();

        // Act  
        var manager = new PerformanceLoggerManager(logger);

        // Assert
        Assert.IsNotNull(manager);
        Assert.IsNotNull(manager.PerformanceLogger);
    }


    [Test]
    public void TestNoLogging()
    {
        // Arrange 
        _delegateWasFired = false;

        var logger = new PerformanceLogger();

        var manager = new PerformanceLoggerManager(logger)
        {
            StatusMessageDelegate = StatusMessageDelegate
        };

        Assert.IsNotNull(manager);
        Assert.IsNotNull(manager.PerformanceLogger);

        // Act  
        Thread.Sleep(2000);

        // Assert
        Assert.IsFalse(_delegateWasFired);

    }

    [Test]
    public void TestStartLogging()
    {
        // Arrange 
        _delegateWasFired = false;

        var logger = new PerformanceLogger();

        var manager = new PerformanceLoggerManager(logger)
        {
            StatusMessageDelegate = StatusMessageDelegate
        };

        Assert.IsNotNull(manager);
        Assert.IsNotNull(manager.PerformanceLogger);

        // Act  
        manager.StartLogging();
        Thread.Sleep(2000);

        // Assert
        Assert.IsTrue(_delegateWasFired);

    }


    private void StatusMessageDelegate(string message)
    {
        _delegateWasFired = true;
    }
}