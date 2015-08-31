// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStopwatch.cs" company="Labo">
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
//   Stopwatch interface that provides a set of methods and properties that you can use to accurately measure elapsed time.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Diagnostics
{
    using System;

    /// <summary>
    /// Stopwatch interface that provides a set of methods and properties that you can use to accurately measure elapsed time.
    /// </summary>
    public interface IStopwatch
    {
        /// <summary>
        /// Gets the frequency of the stopwatch.
        /// </summary>
        long Frequency { get; }

        /// <summary>
        /// Gets the total elapsed time measured by the current instance.
        /// </summary>
        /// <value>
        /// A read-only System.TimeSpan representing the total elapsed time measured by the current instance.
        /// </value>
        TimeSpan Elapsed { get; }

        /// <summary>
        /// Gets the total elapsed time measured by the current instance, in milliseconds.
        /// </summary>
        /// <value>
        /// A read-only long integer representing the total number of milliseconds measured by the current instance.
        /// </value>
        long ElapsedMilliseconds { get; }

        /// <summary>
        /// Gets the total elapsed time measured by the current instance, in timer ticks.
        /// </summary>
        /// <value>
        /// A read-only long integer representing the total number of timer ticks measured by the current instance.
        /// </value>
        long ElapsedTicks { get; }

        /// <summary>
        /// Gets a value indicating whether the Stopwatch timer is running.
        /// </summary>
        /// <value>
        /// <c>true</c> if the Stopwatch instance is currently running and measuring elapsed time for an interval; otherwise, <c>false</c>.
        /// </value>
        bool IsRunning { get; }

        /// <summary>
        /// Stops time interval measurement and resets the elapsed time to zero.
        /// </summary>
        void Reset();

        /// <summary>
        /// Stops time interval measurement, resets the elapsed time to zero, and starts measuring elapsed time.
        /// </summary>
        void Restart();

        /// <summary>
        /// Starts, or resumes, measuring elapsed time for an interval.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops measuring elapsed time for an interval.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Stop")]
        void Stop(); 
    }
}