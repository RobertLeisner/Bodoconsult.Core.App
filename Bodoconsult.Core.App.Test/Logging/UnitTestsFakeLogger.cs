// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using Bodoconsult.Core.App.Logging;
using Microsoft.Extensions.Logging;

namespace Bodoconsult.Core.App.Test.Logging;

[TestFixture]
internal class UnitTestsFakeLogger: BaseFakeLoggerTests
{

    [SetUp]
    public void Setup()
    {
        LoggedMessages.Clear();
    }


    [Test]
    public void TestLog()
    {
        // Arrange 
        var fake = new FakeLogger("TestCategrory")
        {
            FakeLogDelegate = FakeLogDelegate
        };

        logger = fake;

        // Act  
        logger.Log(LogLevel.Critical, "Hallo");

        // Assert
        Assert.IsTrue(LoggedMessages.Count == 1);

    }


}