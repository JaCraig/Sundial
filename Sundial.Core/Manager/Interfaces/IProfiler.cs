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

using System;

namespace Sundial.Core.Manager.Interfaces
{
    /// <summary>
    /// Profiler interface
    /// </summary>
    public interface IProfiler : IDisposable
    {
        /// <summary>
        /// Starts profiling, saving the information to the name specified
        /// </summary>
        /// <param name="functionName">Name of the profiler</param>
        /// <returns>IDisposable that will stop the profiler when disposed of</returns>
        IDisposable Profile(string functionName);

        /// <summary>
        /// Starts profiling
        /// </summary>
        /// <returns>Starts profiling</returns>
        IDisposable StartProfiling();

        /// <summary>
        /// Stops profiling and returns the information captured
        /// </summary>
        /// <param name="discardResults">Determines if the results are kept or discarded</param>
        /// <returns>The results from the profiling</returns>
        IProfilerResult StopProfiling(bool discardResults);
    }
}