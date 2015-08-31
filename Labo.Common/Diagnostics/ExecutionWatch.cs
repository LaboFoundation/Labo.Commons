// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExecutionWatch.cs" company="Labo">
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
//   Defines the ExecutionWatch type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Diagnostics
{
    using System;

    using Labo.Common.Resources;

    /// <summary>
    /// Execution watch class.
    /// </summary>
    public sealed class ExecutionWatch
    {
        /// <summary>
        /// The stopwatch instance.
        /// </summary>
        private readonly IStopwatch m_Stopwatch;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionWatch"/> class.
        /// </summary>
        /// <param name="stopwatch">The stopwatch.</param>
        public ExecutionWatch(IStopwatch stopwatch)
        {
            m_Stopwatch = stopwatch;
        }

        /// <summary>
        /// Measures actions execution time.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="executionCount">The execution count.</param>
        /// <returns>Execution time.</returns>
        public TimeSpan Measure(Action action, int executionCount = 1)
        {
            return Measure(null, action, null, executionCount);
        }

        /// <summary>
        /// Measures actions execution time.
        /// </summary>
        /// <param name="onStart">The action that is invoked before the main action's invocation.</param>
        /// <param name="action">The action.</param>
        /// <param name="onFinish">The action that is invoked after the main action's invocation.</param>
        /// <param name="executionCount">The actions execution count.</param>
        /// <returns>Execution time.</returns>
        /// <exception cref="System.ArgumentNullException">action</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">executionCount;Execution count must be bigger than 0.</exception>
        public TimeSpan Measure(Action onStart, Action action, Action onFinish, int executionCount = 1)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            if (executionCount < 1)
            {
                throw new ArgumentOutOfRangeException("executionCount", Strings.ExecutionWatch_Measure_Execution_count_must_be_bigger_than_zero);
            }

            if (onStart != null)
            {
                onStart();
            }

            try
            {
                m_Stopwatch.Reset();
                m_Stopwatch.Start();

                for (int i = 0; i < executionCount; i++)
                {
                    action();
                }
            }
            finally 
            {
                m_Stopwatch.Stop();
            }

            if (onFinish != null)
            {
                onFinish();
            }

            return m_Stopwatch.Elapsed;
        }
    }
}
