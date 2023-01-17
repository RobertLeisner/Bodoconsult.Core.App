// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.
// Licence MIT

using Bodoconsult.Core.App.AppStarter;
using Bodoconsult.Core.App.Interfaces;

namespace Bodoconsult.Core.App.Test.AppStarter;

[TestFixture]
internal class UnitTestBaseAppStarterUi
{

    [Test]
    public void TestCtor()
    {
        // Arrange 
        IAppStarterProcessHandler h = new FakeAppStarterProcessHandler();

        // Act  

        var b = new BaseAppStarterUi(h);

        // Assert
        Assert.IsNotNull(b.AppStarterProcessHandler);
        Assert.AreEqual(h, b.AppStarterProcessHandler);
    }


    [Test]
    public void TestIsAnotherInstance()
    {
        // Arrange 
        var h = new FakeAppStarterProcessHandler();

        var b = new BaseAppStarterUi(h);

        // Act  
        var result = b.IsAnotherInstance;

        // Assert
        Assert.IsFalse(result);

    }


    [Test]
    public void TestStart()
    {
        // Arrange 
        var h  = new FakeAppStarterProcessHandler();

        var b = new BaseAppStarterUi(h);

        Assert.IsFalse(h.WasStartApplication);

        // Act  
        b.Start();

        // Assert
        Assert.IsTrue(h.WasStartApplication);

    }
        

}