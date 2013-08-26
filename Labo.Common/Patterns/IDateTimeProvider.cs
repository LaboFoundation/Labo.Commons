using System;

namespace Labo.Common.Patterns
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDateTimeProvider
    {
        /// <summary>
        /// Gets the UTC now.
        /// </summary>
        /// <value>
        /// The UTC now.
        /// </value>
        DateTime UtcNow { get; }

        /// <summary>
        /// Gets the now.
        /// </summary>
        /// <value>
        /// The now.
        /// </value>
        DateTime Now { get; }

        /// <summary>
        /// Gets the today.
        /// </summary>
        /// <value>
        /// The today.
        /// </value>
        DateTime Today { get; }
    }
}