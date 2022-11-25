// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Core.App.Logging;

namespace Bodoconsult.Core.App.Test.Logging
{
    [TestFixture]
    internal class UnitTestsAppLoggerProxy : BaseFakeLoggerTests
    {
        private AppLoggerProxy _log;


        [SetUp]
        public void Setup()
        {
            LoggedMessages.Clear();
        }

        //[Test]
        //public void TestCheckQueueWithExternalDelegateSuccess()
        //{
        //    // Arrange 
        //    var appLoggerFactory = new FakeLoggerFactory
        //    {
        //        FakeLogDelegate = FakeLogDelegate
        //    };

        //    _log = new AppLoggerProxy(appLoggerFactory);

        //    // Act
        //    _log.LogWarning("Hallo");
        //    var task = Task.Delay(2 * AppLoggerProxy.DelayTimeQueueAccess);
        //    task.Wait();

        //    // Assert
        //    Assert.IsTrue(LoggedMessages.Count == 1);

        //    appLoggerFactory.Dispose();
        //}

        //[Test]
        //public void TestCheckQueueWithInternalDelegateSuccess()
        //{
        //    // Arrange 
        //    var appLoggerFactory = new FakeLoggerFactory();
        //    _log = new AppLoggerProxy(appLoggerFactory);

        //    // Act
        //    _log.LogWarning("Hallo");
        //    var task = Task.Delay(2 * AppLoggerProxy.DelayTimeQueueAccess);
        //    task.Wait();

        //    // Assert
        //    Assert.IsTrue(appLoggerFactory.LoggedMessages.Count == 1);

        //    appLoggerFactory.Dispose();
        //}

        //[Test]
        //public void TestCheckQueueWithInternalDelegateSuccessMultipleLogs()
        //{
        //    // Arrange 
        //    var queryLogger = new FakeLoggerFactory();
        //    _log = new AppLoggerProxy(queryLogger);

        //    // Act
        //    _log.LogWarning("Hallo");
        //    //_log.CheckQueue();

        //    _log.LogWarning("Hallo");
        //    //_log.CheckQueue();

        //    _log.LogWarning("Hallo");
        //    //_log.CheckQueue();

        //    var task = Task.Delay(2 * AppLoggerProxy.DelayTimeQueueAccess);
        //    task.Wait();

        //    // Assert
        //    Assert.IsTrue(queryLogger.LoggedMessages.Count == 3);
        //    queryLogger.Dispose();
        //}


        [Test]
        public void TestCheckQueueWithExternalDelegateNoSuccess()
        {
            // Arrange 
            var appLogger = new FakeLoggerFactory
            {
                FakeLogDelegate = FakeLogDelegate
            };

            _log = new AppLoggerProxy(appLogger);

            // Act
            _log.LogInformation("Hallo");
            var task = Task.Delay(2 * AppLoggerProxy.DelayTimeQueueAccess);
            task.Wait();

            // Assert
            Assert.IsTrue(LoggedMessages.Count == 0);
            appLogger.Dispose();
        }

        [Test]
        public void TestCheckQueueWithInternalDelegateNoSuccess()
        {
            // Arrange 
            var appLogger = new FakeLoggerFactory();
            _log = new AppLoggerProxy(appLogger);

            // Act
            _log.LogInformation("Hallo");
            var task = Task.Delay(2 * AppLoggerProxy.DelayTimeQueueAccess);
            task.Wait();

            // Assert
            Assert.IsTrue(appLogger.LoggedMessages.Count == 0);
            appLogger.Dispose();
        }


        [Test]
        public void TestFormatArgsString()
        {
            // Arrange 
            var input = "test";

            // Act  
            var result = AppLoggerProxy.FormatArgs(new object[] { input });

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(string.IsNullOrEmpty(result));

        }

        [Test]
        public void TestFormatArgsObject()
        {
            // Arrange 
            var input = new LoggingConfig();

            // Act  
            var result = AppLoggerProxy.FormatArgs(new object[] { input });

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(string.IsNullOrEmpty(result));

        }


        [Test]
        public void TestFormatArgsException()
        {
            // Arrange 
            var input = new ArgumentException("Hallo");

            // Act  
            var result = AppLoggerProxy.FormatArgs(new object[] { input });

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(string.IsNullOrEmpty(result));

        }

    }
}

