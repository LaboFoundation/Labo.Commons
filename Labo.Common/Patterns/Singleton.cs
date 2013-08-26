using System;

namespace Labo.Common.Patterns
{
    /// <summary>
    /// Generic Singleton Pattern Implementation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class Singleton<T> where T : class, new()
    {
        private static readonly Lazy<T> s_Instance = new Lazy<T>(() => new T(), true);

        /// <summary>
        /// Gets the Singleton instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static T Instance { get { return s_Instance.Value; } }
    }
}