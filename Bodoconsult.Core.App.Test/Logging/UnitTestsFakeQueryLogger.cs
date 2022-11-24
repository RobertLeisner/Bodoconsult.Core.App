// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

//using Microsoft.Extensions.Logging;
//using NUnit.Framework;
//using Bodoconsult EDV-Dienstleistungen GmbH.StSys.SQL.Model.Logging;

//namespace Bodoconsult EDV-Dienstleistungen GmbH.StSys.SQL.StSysDB.Tests.Logging
//{
//    [TestFixture]
//    internal class UnitTestsFakeAppLogger: BaseFakeLoggerTests
//    {

//        [SetUp]
//        public void Setup()
//        {
//            LoggedMessages.Clear();
//        }


//        [Test]
//        public void TestCreateLogger()
//        {
//            // Arrange 
//            var query = new FakeAppLoggerFactory
//            {
//                FakeLogDelegate = FakeLogDelegate
//            };

//            // Act
//            Assert.IsNotNull(query);

//            var factory = query.Create();

//            Assert.IsNotNull(factory);

//            var fake = (FakeLogger)factory.CreateLogger("TestCategrory");

//            Assert.IsNotNull(fake);
           
//            logger = fake;
//            logger.Log(LogLevel.Critical, "Hallo");

//            // Assert
//            Assert.IsTrue(LoggedMessages.Count == 1);

//        }
//    }
//}