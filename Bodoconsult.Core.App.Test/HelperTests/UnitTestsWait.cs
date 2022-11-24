// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Diagnostics;
using Bodoconsult.Core.App.Helpers;

namespace Bodoconsult.Core.App.Test.HelperTests
{
    [TestFixture]
    public class UnitTestsWait
    {


        [Test]
        public void TestUntilTimeout()
        {
            // Arrange 
            const int timeout = 4500;
            
            var watch = Stopwatch.StartNew();


            // Act  
            Wait.Until(() => false, timeout);
            watch.Stop();

            // Assert
            Assert.IsTrue(watch.ElapsedMilliseconds>= timeout);
            
        }


        [Test]
        public void TestUntilPredicate()
        {
            // Arrange 
            const int timeout = 4500;

            var watch = Stopwatch.StartNew();

            // Act  
            Wait.Until(() => true, timeout);
            watch.Stop();

            // Assert
            Assert.IsTrue(watch.ElapsedMilliseconds < timeout);

        }

    }
}