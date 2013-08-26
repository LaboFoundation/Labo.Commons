namespace Labo.Common.Patterns
{
    /// <summary>
    /// 
    /// </summary>
    public static class DateTimeProvider
    {
        private static IDateTimeProvider s_DateTimeProvider = new DefaultDateTimeProvider();

        /// <summary>
        /// Gets the current DateTimeProvider.
        /// </summary>
        /// <value>
        /// The current.
        /// </value>
        public static IDateTimeProvider Current
        {
            get { return s_DateTimeProvider; }
        }

        internal static void ResetDateTimeProvider()
        {
            lock (s_DateTimeProvider)
            {
                s_DateTimeProvider = new DefaultDateTimeProvider();                
            }
        }

        internal static void SetDateTimeProvider(IDateTimeProvider dateTimeProvider)
        {
            lock (s_DateTimeProvider)
            {
                s_DateTimeProvider = dateTimeProvider;
            }
        }
    }
}