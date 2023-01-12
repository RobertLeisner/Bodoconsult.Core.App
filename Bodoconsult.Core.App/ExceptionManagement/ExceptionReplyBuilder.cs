// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.
// Licence MIT

using Bodoconsult.Core.App.BusinessTransactions.Replies;
using Bodoconsult.Core.App.Interfaces;

namespace Bodoconsult.Core.App.ExceptionManagement
{
    /// <summary>
    /// Current exception reply builder impl
    /// </summary>
    public class ExceptionReplyBuilder : IExceptionReplyBuilder
    {
        /// <summary>
        /// Default error code
        /// </summary>
        public int DefaultErrorCode { get; set; } = 999999999;


        /// <summary>
        /// Exception replies currently loaded for usage in the app
        /// </summary>
        public Dictionary<string, ExceptionReplyData> ExceptionReplies { get; } = new();

        /// <summary>
        /// Create a reply for an exception that has happened
        /// </summary>
        /// <param name="exception">Exception that has been thrown</param>
        /// <returns>A newly created <see cref="DefaultBusinessTransactionReply"/> instance</returns>
        public IBusinessTransactionReply CreateReply(Exception exception)
        {

            var eName = exception.GetType().Name;

            DefaultBusinessTransactionReply reply;

            // Search data for the current exception
            var success = ExceptionReplies.TryGetValue(eName, out var eData);

            // If no value found return default reply
            if (!success)
            {
                reply = new DefaultBusinessTransactionReply
                {
                    ErrorCode = DefaultErrorCode, //  StSysErrorCodes.ExceptionOccursCode
                    ExceptionMessage = exception.StackTrace,
                    Message = $"Exception message: {exception.Message}"
                };

                return reply;
            }

            // Return a customized reply
            reply = new DefaultBusinessTransactionReply
            {
                ErrorCode = eData.ErrorCode == 0 ? DefaultErrorCode : eData.ErrorCode,
                ExceptionMessage = eData.EmptyErrorMessage ? "" : exception.StackTrace,
                Message = $"Exception message: {exception.Message} {eData.Message}"
            };

            return reply;
        }

        /// <summary>
        /// Add an <see cref="IExceptionReplyProvider"/> instance
        /// </summary>
        /// <param name="exceptionReplyProvider">Provider for exception replies to add</param>
        public void AddProvider(IExceptionReplyProvider exceptionReplyProvider)
        {
            foreach (var data in exceptionReplyProvider.ExceptionReplies)
            {
                var success = ExceptionReplies.TryAdd(data.Key, data.Value);
                // ToDo: logging
            }
        }
    }
}