// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StopwatchContext.cs" company="Labo">
//   The MIT License (MIT)
//   
//   Copyright (c) 2013 Bora Akgun
//   
//   Permission is hereby granted, free of charge, to any person obtaining a copy of
//   this software and associated documentation files (the "Software"), to deal in
//   the Software without restriction, including without limitation the rights to
//   use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
//   the Software, and to permit persons to whom the Software is furnished to do so,
//   subject to the following conditions:
//   
//   The above copyright notice and this permission notice shall be included in all
//   copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
//   FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
//   COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
//   IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//   CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   The stopwatch context class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Diagnostics
{
    using System;
    using System.Diagnostics;
    using System.Reflection;

    /// <summary>
    /// The stopwatch context class.
    /// </summary>
    public sealed class StopwatchContext : IDisposable
    {
        /// <summary>
        /// The stopwatch
        /// </summary>
        private IStopwatch m_Stopwatch;

        /// <summary>
        /// The action that is invoked when this instance is disposed
        /// </summary>
        private StopwatchContextOnDispose m_OnDispose;

        /// <summary>
        /// The execution method name
        /// </summary>
        private string m_MethodName;

        /// <summary>
        /// The disposed flag
        /// </summary>
        private bool m_Disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="StopwatchContext"/> class.
        /// </summary>
        /// <param name="stopwatch">The stopwatch.</param>
        /// <param name="onDispose">The configuration dispose.</param>
        public StopwatchContext(IStopwatch stopwatch, StopwatchContextOnDispose onDispose)
        {
            if (stopwatch == null)
            {
                throw new ArgumentNullException("stopwatch");
            }

            if (onDispose == null)
            {
                throw new ArgumentNullException("onDispose");
            }

            m_Stopwatch = stopwatch;
            m_OnDispose = onDispose;

            StackFrame frame = new StackFrame(1, false);
            MethodBase method = frame.GetMethod();
            m_MethodName = method.ToString();

            m_Stopwatch.Reset();
            m_Stopwatch.Start();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="StopwatchContext"/> class. Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.
        /// </summary>
        ~StopwatchContext()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            try
            {
                m_Stopwatch.Stop();
                m_OnDispose(m_MethodName, m_Stopwatch.Elapsed);
            }
            finally 
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            if (m_Disposed)
            {
                return;
            }

            if (disposing)
            {
                m_Stopwatch = null;
                m_OnDispose = null;
                m_MethodName = null;
                m_Disposed = true;
            }
        }
    }
}