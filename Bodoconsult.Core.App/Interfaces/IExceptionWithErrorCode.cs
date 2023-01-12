// Copyright (c) Royotech. All rights reserved.
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.
// Licence MIT

namespace Bodoconsult.Core.App.Interfaces
{
    /// <summary>
    /// Interface for exceptions providing an error code
    /// </summary>
    public interface IExceptionWithErrorCode
    {

        /// <summary>
        /// Error code provide by the exception
        /// </summary>
        public int ErrorCode { get;  }

    }
}