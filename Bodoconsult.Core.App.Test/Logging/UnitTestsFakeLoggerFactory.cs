// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Core.App.Logging;
using Microsoft.Extensions.Logging;

namespace Bodoconsult.Core.App.Test.Logging
{
    [TestFixture]
    internal class UnitTestsFakeLoggerFactory: BaseFakeLoggerTests
    {

        [SetUp]
        public void Setup()
        {
            LoggedMessages.Clear();
        }

        [Test]
        public void TestCreateLogger()
        {
            // Arrange 
            var factory = new FakeLoggerFactory
            {
                FakeLogDelegate = FakeLogDelegate
            };

            // Act  
            var fake = (FakeLogger)factory.CreateLogger("TestCategrory");
            
            Assert.IsNotNull(fake);
            Assert.IsTrue(LoggedMessages.Count == 0);

            var logger = (ILogger)fake;
            logger.Log(LogLevel.Critical, "Hallo");

            // Assert
            Assert.IsTrue(LoggedMessages.Count == 1);

            factory.Dispose();
        }
    }
}