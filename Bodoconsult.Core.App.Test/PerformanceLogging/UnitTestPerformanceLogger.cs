// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.Diagnostics;
using Bodoconsult.Core.App.PerformanceLogging;

namespace Bodoconsult.Core.App.Test.PerformanceLogging
{
    [TestFixture]
    public class UnitTestPerformanceLogger
    {

        [Test]
        public void TestGetCountersAsString()
        {

            // Arrange 
            var logger = new PerformanceLogger();

            // Act  
            var s = logger.GetCountersAsString();

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(s));
            Debug.Print(s);

        }
    }
}