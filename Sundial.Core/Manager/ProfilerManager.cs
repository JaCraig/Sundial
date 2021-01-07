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

using BigBook.Patterns.BaseClasses;
using Sundial.Core.Manager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sundial.Core.Manager
{
    /// <summary>
    /// Profiler manager
    /// </summary>
    /// <seealso cref="SafeDisposableBaseClass"/>
    public class ProfilerManager : SafeDisposableBaseClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProfilerManager"/> class.
        /// </summary>
        /// <param name="profilers">The profilers.</param>
        public ProfilerManager(IEnumerable<IProfiler> profilers)
        {
            profilers ??= new List<IProfiler>();
            Profiler = profilers.FirstOrDefault(x => !x.GetType().Namespace.StartsWith("SUNDIAL", StringComparison.OrdinalIgnoreCase))
                     ?? profilers.FirstOrDefault(x => x.GetType().Namespace.StartsWith("SUNDIAL", StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Root profiler object
        /// </summary>
        /// <value>The profiler.</value>
        protected IProfiler Profiler { get; private set; }

        /// <summary>
        /// Starts the profiler and uses the name specified
        /// </summary>
        /// <param name="name">Name of the entry</param>
        /// <returns>An IDisposable object that will stop the profiler when disposed of</returns>
        public IDisposable Profile(string name)
        {
            return Profiler.Profile(name);
        }

        /// <summary>
        /// Starts profiling
        /// </summary>
        /// <returns>Starts profiling</returns>
        public IDisposable StartProfiling()
        {
            return Profiler.StartProfiling();
        }

        /// <summary>
        /// Ends profiling
        /// </summary>
        /// <param name="discardResults">Determines if the results should be discarded</param>
        /// <returns>Result of the profiling</returns>
        public IProfilerResult StopProfiling(bool discardResults)
        {
            return Profiler.StopProfiling(discardResults);
        }

        /// <summary>
        /// Outputs the profiler information as a string
        /// </summary>
        /// <returns>The profiler information as a string</returns>
        public override string ToString()
        {
            return string.Format("Profilers: {0}\r\n", Profiler);
        }

        /// <summary>
        /// Disposes of the object
        /// </summary>
        /// <param name="Managed">
        /// Determines if all objects should be disposed or just managed objects
        /// </param>
        protected override void Dispose(bool Managed)
        {
            if (Profiler != null)
            {
                Profiler.Dispose();
                Profiler = null;
            }
        }
    }
}