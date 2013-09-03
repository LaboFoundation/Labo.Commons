using System;

namespace Labo.Common.Patterns
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class FuncDateTimeProvider : IDateTimeProvider
    {
        private readonly Func<DateTime> m_FuncDateTime;

        /// <summary>
        /// Gets the UTC now.
        /// </summary>
        /// <value>
        /// The UTC now.
        /// </value>
        public DateTime UtcNow { get { return DateTime.SpecifyKind(m_FuncDateTime(), DateTimeKind.Utc); } }
        
        /// <summary>
        /// Gets the now.
        /// </summary>
        /// <value>
        /// The now.
        /// </value>
        public DateTime Now { get { return m_FuncDateTime(); } }

        /// <summary>
        /// Gets the today.
        /// </summary>
        /// <value>
        /// The today.
        /// </value>
        public DateTime Today { get { return m_FuncDateTime().Date; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="FuncDateTimeProvider"/> class.
        /// </summary>
        /// <param name="funcDateTime">The func date time.</param>
        /// <exception cref="System.ArgumentNullException">funcDateTime</exception>
        public FuncDateTimeProvider(Func<DateTime> funcDateTime)
        {
            if (funcDateTime == null) throw new ArgumentNullException("funcDateTime");

            m_FuncDateTime = funcDateTime;
        }
    }
}