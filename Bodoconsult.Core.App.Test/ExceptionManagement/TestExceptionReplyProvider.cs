// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.
// Licence MIT

using Bodoconsult.Core.App.ExceptionManagement;
using Bodoconsult.Core.App.Interfaces;

namespace Bodoconsult.Core.App.Test.ExceptionManagement
{
    internal class TestExceptionReplyProvider : IExceptionReplyProvider
    {

        public TestExceptionReplyProvider()
        {
            //
            var e = new ExceptionReplyData
            {
                Message =  "Argument may NOT be null!"
            };

            ExceptionReplies.Add(nameof(ArgumentNullException), e);

            //
            e = new ExceptionReplyData
            {
                Message = "Test exception!"
            };

            ExceptionReplies.Add(nameof(TestException), e);

        }

        public Dictionary<string, ExceptionReplyData> ExceptionReplies { get; } = new();
    }
}