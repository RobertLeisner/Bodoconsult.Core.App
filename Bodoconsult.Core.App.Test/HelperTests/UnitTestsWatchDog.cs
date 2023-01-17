// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Core.App.Helpers;
using Bodoconsult.Core.App.Interfaces;

namespace Bodoconsult.Core.App.Test.HelperTests;

[TestFixture]
internal class UnitTestsWatchDog
{
    private bool _isFired;
    private int _firedCount;

    [SetUp]
    public void TestSetup()
    {
        _isFired = false;
        _firedCount = 0;
    }


    /// <summary>
    /// Runner method for the watchdog
    /// </summary>
    private void Runner()
    {
        _isFired = true;
        _firedCount++;
    }

    /// <summary>
    /// Runner emthod for the watchdog
    /// </summary>
    private async void RunnerAsync()
    {
        await Task.Run(() =>
        {
            _isFired = true;
            _firedCount++;
        });

    }


    [Test]
    public void TestCtor()
    {
        // Arrange 
        _isFired = false;
        _firedCount = 0;
        const int delayTime = 1000;

        WatchDogRunnerDelegate runner = Runner;

        var w = new WatchDog(runner, delayTime);

        // Act  
        Thread.Sleep((int)(delayTime * 1.5));

        // Assert
        Assert.AreEqual(runner, w.WatchDogRunnerDelegate);
        Assert.AreEqual(delayTime, w.DelayUntilNextRunnerFired);
        Assert.IsFalse(_isFired);

    }

    [Test]
    public void TestStart()
    {
        // Arrange 
        _isFired = false;
        const int delayTime = 1000;

        WatchDogRunnerDelegate runner = Runner;

        var w = new WatchDog(runner, delayTime);
        w.StartWatchDog();

        // Act  
        Thread.Sleep((int)(delayTime * 1.5));

        // Assert
        w.StopWatchDog();
        Assert.IsTrue(_isFired);
        Assert.IsTrue( _firedCount>1);
    }

    [Test]
    public void TestStart2TimesFired()
    {
        // Arrange 
        _isFired = false;
        const int delayTime = 1000;

        WatchDogRunnerDelegate runner = Runner;

        var w = new WatchDog(runner, delayTime);
        w.StartWatchDog();

        // Act  
        Thread.Sleep((int)(delayTime * 2.5));

        // Assert
        w.StopWatchDog();
        Assert.IsTrue(_isFired);
        Assert.IsTrue(_firedCount > 1);
    }


    [Test]
    public void TestStartAsync()
    {
        // Arrange 
        _isFired = false;
        const int delayTime = 1000;

        WatchDogRunnerDelegate runner = RunnerAsync;

        var w = new WatchDog(runner, delayTime);
        w.StartWatchDog();

        // Act  
        Thread.Sleep((int)(delayTime * 1.5));

        // Assert
        w.StopWatchDog();
        Assert.IsTrue(_isFired);
        Assert.IsTrue(_firedCount > 1);
    }


    [Test]
    public void TestRestart()
    {
        // Arrange 
        _isFired = false;
        const int delayTime = 1000;

        WatchDogRunnerDelegate runner = Runner;

        var w = new WatchDog(runner, delayTime);

        // Act  1
        w.StartWatchDog();
        Thread.Sleep((int)(delayTime * 2.5));
        w.StopWatchDog();

        // Act  2
        w.StartWatchDog();
        Thread.Sleep((int)(delayTime * 2.5));
        w.StopWatchDog();

        // Act  3
        w.StartWatchDog();
        Thread.Sleep((int)(delayTime * 2.5));
        w.StopWatchDog();

        // Assert

        Assert.IsTrue(_isFired);
        Assert.IsTrue(_firedCount>12);
    }

}