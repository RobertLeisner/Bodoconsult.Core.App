// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Core.App.EventCounters;
using Bodoconsult.Core.App.Factories;

namespace Bodoconsult.Core.App.Test.Factories;

[TestFixture]
internal class UnitTestFakeAppEventSourceFactory
{

    [Test]
    public void TestCreateInstance()
    {
        // Arrange 
        var f = new FakeAppEventSourceFactory();

        // Act  
        var inst = f.CreateInstance();

        // Assert
        Assert.IsNotNull(inst);
        Assert.That(inst, Is.TypeOf(typeof(FakeAppEventSource)));

    }
        
}