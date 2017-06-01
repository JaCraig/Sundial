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
using Sundial.Core.Manager;
using Sundial.Core.Manager.Interfaces;
using System;

namespace Sundial.Core
{
    /// <summary>
    /// Profiler object
    /// </summary>
    /// <seealso cref="SafeDisposableBaseClass"/>
    public class Profiler : SafeDisposableBaseClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Profiler"/> class.
        /// </summary>
        /// <param name="functionName">Name of the function.</param>
        public Profiler(string functionName)
        {
            ProfilerObject = Canister.Builder.Bootstrapper.Resolve<ProfilerManager>().Profile(functionName);
        }

        /// <summary>
        /// Profiler Object
        /// </summary>
        /// <value>The profiler object.</value>
        private IDisposable ProfilerObject { get; set; }

        /// <summary>
        /// Starts profiling
        /// </summary>
        /// <returns>The profiler object</returns>
        public static IDisposable StartProfiling()
        {
            return Canister.Builder.Bootstrapper.Resolve<ProfilerManager>().StartProfiling();
        }

        /// <summary>
        /// Stops profiling and returns the result
        /// </summary>
        /// <param name="discardResults">Determines if the results should be discarded or not</param>
        /// <returns>Result of the profiling</returns>
        public static IProfilerResult StopProfiling(bool discardResults)
        {
            return Canister.Builder.Bootstrapper.Resolve<ProfilerManager>().StopProfiling(discardResults);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return ProfilerObject != null ? ProfilerObject.ToString() : "";
        }

        /// <summary>
        /// Function to override in order to dispose objects
        /// </summary>
        /// <param name="Managed">
        /// If true, managed and unmanaged objects should be disposed. Otherwise unmanaged objects only.
        /// </param>
        protected override void Dispose(bool Managed)
        {
            if (ProfilerObject != null)
            {
                ProfilerObject.Dispose();
                ProfilerObject = null;
            }
        }
    }
}