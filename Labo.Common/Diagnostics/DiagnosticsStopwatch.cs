// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DiagnosticsStopwatch.cs" company="Labo">
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
//   Diagnostics stopwatch implementation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Diagnostics
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Diagnostics stopwatch implementation.
    /// </summary>
    internal sealed class DiagnosticsStopwatch : IStopwatch
    {
        /// <summary>
        /// The stopwatch.
        /// </summary>
        private readonly Stopwatch m_Stopwatch;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticsStopwatch"/> class.
        /// </summary>
        public DiagnosticsStopwatch()
        {
            m_Stopwatch = new Stopwatch();
        }

        /// <summary>
        /// Gets the total elapsed time measured by the current instance.
        /// </summary>
        /// <value>
        /// A read-only System.TimeSpan representing the total elapsed time measured by the current instance.
        /// </value>
        public TimeSpan Elapsed
        {
            get { return m_Stopwatch.Elapsed; }
        }

        /// <summary>
        /// Gets the total elapsed time measured by the current instance, in milliseconds.
        /// </summary>
        /// <value>
        /// A read-only long integer representing the total number of milliseconds measured by the current instance.
        /// </value>
        public long ElapsedMilliseconds
        {
            get { return m_Stopwatch.ElapsedMilliseconds; }
        }

        /// <summary>
        /// Gets the total elapsed time measured by the current instance, in timer ticks.
        /// </summary>
        /// <value>
        /// A read-only long integer representing the total number of timer ticks measured by the current instance.
        /// </value>
        public long ElapsedTicks
        {
            get { return m_Stopwatch.ElapsedTicks; }
        }

        /// <summary>
        /// Gets a value indicating whether the Stopwatch timer is running.
        /// </summary>
        /// <value>
        /// <c>true</c> if the Stopwatch instance is currently running and measuring elapsed time for an interval; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunning
        {
            get { return m_Stopwatch.IsRunning; }
        }

        /// <summary>
        /// Stops time interval measurement and resets the elapsed time to zero.
        /// </summary>
        public void Reset()
        {
            m_Stopwatch.Reset();
        }

        /// <summary>
        /// Stops time interval measurement, resets the elapsed time to zero, and starts measuring elapsed time.
        /// </summary>
        public void Restart()
        {
            m_Stopwatch.Restart();
        }

        /// <summary>
        /// Starts, or resumes, measuring elapsed time for an interval.
        /// </summary>
        public void Start()
        {
            m_Stopwatch.Start();
        }

        /// <summary>
        /// Stops measuring elapsed time for an interval.
        /// </summary>
        public void Stop()
        {
            m_Stopwatch.Stop();
        }

        /// <summary>
        /// Gets the frequency of the stopwatch.
        /// </summary>
        public long Frequency
        {
            get { return Stopwatch.Frequency; }
        }
    }
}