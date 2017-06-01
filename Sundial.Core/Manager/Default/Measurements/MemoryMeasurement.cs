/*
Copyright 2017 James Craig

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using Sundial.Core.Manager.Interfaces;
using System;

namespace Sundial.Core.Manager.Default.Measurements
{
    /// <summary>
    /// Memory measurement
    /// </summary>
    /// <seealso cref="IMeasurement"/>
    public class MemoryMeasurement : IMeasurement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryMeasurement"/> class.
        /// </summary>
        /// <param name="timer">The timer.</param>
        public MemoryMeasurement()
        {
            if (Timer == null)
            {
                lock (LockObject)
                {
                    if (Timer == null)
                    {
                        Timer = new StopWatch();
                        Timer.Start();
                        LastCounterTime = Timer.ElapsedTime;
                        LastValue = GC.GetTotalMemory(false) / 1048576;
                    }
                }
            }
        }

        /// <summary>
        /// The last counter time
        /// </summary>
        private static double LastCounterTime;

        /// <summary>
        /// The last value
        /// </summary>
        private static decimal LastValue;

        /// <summary>
        /// The lock object
        /// </summary>
        private static object LockObject = new object();

        /// <summary>
        /// Gets or sets the counter.
        /// </summary>
        private static StopWatch Timer;

        /// <summary>
        /// Gets the measurement type.
        /// </summary>
        /// <value>The measurement type.</value>
        public string Type => "Memory";

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Starts profiling
        /// </summary>
        /// <returns>Starts profiling</returns>
        public IMeasurement StartProfiling()
        {
            return this;
        }

        /// <summary>
        /// Stops profiling and returns the information captured
        /// </summary>
        /// <param name="discardResults">Determines if the results are kept or discarded</param>
        /// <returns>The results from the profiling</returns>
        public IResultEntry StopProfiling(bool discardResults)
        {
            if (Timer.ElapsedTime >= LastCounterTime + 500)
            {
                LastCounterTime = Timer.ElapsedTime;
                LastValue = GC.GetTotalMemory(false) / 1048576;
            }
            return new Entry(discardResults ? 0 : LastValue);
        }
    }
}