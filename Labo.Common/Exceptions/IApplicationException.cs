namespace Labo.Common.Exceptions
{
    using System;

    /// <summary>
    /// The application exception interface.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
    public interface IApplicationException
    {
        /// <summary>
        /// Gets the exception GUID.
        /// </summary>
        Guid Guid { get; }

        /// <summary>
        /// Gets the exception message.
        /// </summary>
        string Message { get; }
    }
}