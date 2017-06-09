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

namespace Sundial.Core.Manager.Default.Measurements
{
    /// <summary>
    /// Time measurement
    /// </summary>
    /// <seealso cref="IMeasurement"/>
    public class TimeMeasurement : IMeasurement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeMeasurement"/> class.
        /// </summary>
        public TimeMeasurement()
        {
            InternalWatch = new StopWatch();
        }

        /// <summary>
        /// Gets the measurement type.
        /// </summary>
        /// <value>The measurement type.</value>
        public string Type => "Time";

        /// <summary>
        /// Gets the internal watch.
        /// </summary>
        /// <value>The internal watch.</value>
        private StopWatch InternalWatch { get; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            InternalWatch.Stop();
        }

        /// <summary>
        /// Starts profiling
        /// </summary>
        /// <returns>Starts profiling</returns>
        public IMeasurement StartProfiling()
        {
            InternalWatch.Start();
            return this;
        }

        /// <summary>
        /// Stops profiling and returns the information captured
        /// </summary>
        /// <param name="discardResults">Determines if the results are kept or discarded</param>
        /// <returns>The results from the profiling</returns>
        public IResultEntry StopProfiling(bool discardResults)
        {
            InternalWatch.Stop();
            return discardResults ? new Entry(0) : new Entry((decimal)InternalWatch.ElapsedTime);
        }
    }
}