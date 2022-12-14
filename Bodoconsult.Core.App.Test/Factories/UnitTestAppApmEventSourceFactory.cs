// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Core.App.EventCounters;
using Bodoconsult.Core.App.Factories;
using Bodoconsult.Core.App.Logging;

namespace Bodoconsult.Core.App.Test.Factories;

[TestFixture]
internal class UnitTestAppApmEventSourceFactory
{

    [Test]
    public void TestCreateInstance()
    {
        // Arrange
        var logger = new AppLoggerProxy(new FakeLoggerFactory());
        var f = new AppApmEventSourceFactory(logger);

        // Act  
        var inst = f.CreateInstance();

        // Assert
        Assert.IsNotNull(inst);
        Assert.That(inst, Is.TypeOf(typeof(AppApmEventSource)));

    }

}