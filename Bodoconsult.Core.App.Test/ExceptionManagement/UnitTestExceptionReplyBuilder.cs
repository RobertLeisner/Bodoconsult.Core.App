// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.
// Licence MIT

using Bodoconsult.Core.App.ExceptionManagement;
using Bodoconsult.Core.App.Interfaces;

namespace Bodoconsult.Core.App.Test.ExceptionManagement
{
    [TestFixture]
    internal class UnitTestExceptionReplyBuilder
    {

        [Test]
        public void TestCtor()
        {
            // Arrange 

            // Act  
            var e = new ExceptionReplyBuilder();

            // Assert
            Assert.IsNotNull(e.ExceptionReplies);
            Assert.IsFalse(e.ExceptionReplies.Any());

        }

        [Test]
        public void TestAddProvider()
        {
            // Arrange 
            IExceptionReplyProvider p = new TestExceptionReplyProvider();

            IExceptionReplyBuilder e = new ExceptionReplyBuilder();
            
            // Act  
            e.AddProvider(p);

            // Assert
            Assert.IsTrue(e.ExceptionReplies.Any());

        }


        [Test]
        public void TestCreateReply()
        {
            // Arrange 
            IExceptionReplyProvider p = new TestExceptionReplyProvider();

            IExceptionReplyBuilder e = new ExceptionReplyBuilder();
 
            e.AddProvider(p);

            var ex = new ArgumentNullException();

            // Act  
            var reply = e.CreateReply(ex);

            // Assert
            Assert.IsNotNull(reply);

        }
        
    }
}