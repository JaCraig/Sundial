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

using BigBook;
using BigBook.ExtensionMethods;
using Sundial.Core.Manager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sundial.Core.Manager.Default
{
    /// <summary>
    /// Object class used to profile a function. Create at the beginning of a function in a using
    /// statement and it will automatically record the time. Note that this isn't exact and is based
    /// on when the object is destroyed
    /// </summary>
    public class InternalProfiler : IProfiler, IProfilerResult
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="measurements">The measurements.</param>
        /// <param name="cacheManager">The cache manager.</param>
        public InternalProfiler(IEnumerable<IMeasurement> measurements, BigBook.Caching.Manager cacheManager)
        {
            measurements ??= Array.Empty<IMeasurement>();
            Measurements = measurements.ToArray();
            Entries = new ListMapping<string, IResultEntry>();
            Function = "";
            InternalChildren = new Dictionary<string, InternalProfiler>();
            CacheManager = cacheManager;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="functionName">Function/identifier</param>
        /// <param name="measurements">The measurements.</param>
        /// <param name="cacheManager">The cache manager.</param>
        public InternalProfiler(string functionName, IEnumerable<IMeasurement> measurements, BigBook.Caching.Manager cacheManager)
            : this(measurements, cacheManager)
        {
            Parent = Current;
            InternalProfiler Child = Parent?.InternalChildren.ContainsKey(functionName) == true ? Parent.InternalChildren[functionName] : null;
            if (Child == null)
            {
                if (Parent != null)
                    Parent.InternalChildren.Add(functionName, this);
                Function = functionName;
                Level = Parent == null ? 0 : Parent.Level + 1;
                Running = false;
                Current = this;
                Child = this;
            }
            else
            {
                Current = Child;
            }
            Start();
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~InternalProfiler()
        {
            Dispose(false);
        }

        /// <summary>
        /// Contains the current profiler
        /// </summary>
        public static InternalProfiler Current
        {
            get
            {
                var ReturnValue = "Current_Profiler".GetFromCache<InternalProfiler>(cacheName: "Item");
                if (ReturnValue == null)
                {
                    ReturnValue = "Root_Profiler".GetFromCache<InternalProfiler>(cacheName: "Item");
                    Current = ReturnValue;
                }
                return ReturnValue;
            }
            protected set
            {
                value.Cache("Current_Profiler", "Item");
            }
        }

        /// <summary>
        /// Contains the root profiler
        /// </summary>
        public static InternalProfiler Root
        {
            get
            {
                var ReturnValue = "Root_Profiler".GetFromCache<InternalProfiler>(cacheName: "Item");
                if (ReturnValue == null)
                {
                    ReturnValue = new InternalProfiler(
                        "Start",
                        Canister.Builder.Bootstrapper?.ResolveAll<IMeasurement>(),
                        Canister.Builder.Bootstrapper?.Resolve<BigBook.Caching.Manager>());
                    Root = ReturnValue;
                }
                return ReturnValue;
            }
            protected set
            {
                value.Cache("Root_Profiler", "Item");
            }
        }

        /// <summary>
        /// Children result items
        /// </summary>
        public IDictionary<string, IProfilerResult> Children => InternalChildren.ToDictionary(x => x.Key, x => (IProfilerResult)x.Value);

        /// <summary>
        /// Gets the entries.
        /// </summary>
        /// <value>The entries.</value>
        public ListMapping<string, IResultEntry> Entries { get; }

        /// <summary>
        /// Function name
        /// </summary>
        public string Function { get; }

        /// <summary>
        /// Children profiler items
        /// </summary>
        public IDictionary<string, InternalProfiler> InternalChildren { get; }

        /// <summary>
        /// Level of the profiler
        /// </summary>
        protected int Level { get; set; }

        /// <summary>
        /// Parent profiler item
        /// </summary>
        protected InternalProfiler Parent { get; set; }

        /// <summary>
        /// Determines if it is running
        /// </summary>
        protected bool Running { get; set; }

        /// <summary>
        /// Gets the cache manager.
        /// </summary>
        /// <value>The cache manager.</value>
        private BigBook.Caching.Manager CacheManager { get; }

        /// <summary>
        /// Gets the measurements.
        /// </summary>
        /// <value>The measurements.</value>
        private IMeasurement[] Measurements { get; }

        /// <summary>
        /// Compares the profilers and determines if they are not equal
        /// </summary>
        /// <param name="First">First</param>
        /// <param name="Second">Second</param>
        /// <returns>True if they are equal, false otherwise</returns>
        public static bool operator !=(InternalProfiler First, InternalProfiler Second)
        {
            return !(First == Second);
        }

        /// <summary>
        /// Compares the profilers and determines if they are equal
        /// </summary>
        /// <param name="First">First</param>
        /// <param name="Second">Second</param>
        /// <returns>True if they are equal, false otherwise</returns>
        public static bool operator ==(InternalProfiler First, InternalProfiler Second)
        {
            if (First is null && Second is null)
                return true;
            if (First is null)
                return false;
            if (Second is null)
                return false;
            return First.Function == Second.Function;
        }

        /// <summary>
        /// Starts the profiler
        /// </summary>
        public static void Start()
        {
            if (Current == null)
                return;
            if (Current.Running)
            {
                Current.Running = false;
                for (int x = 0; x < Current.Measurements.Length; ++x)
                {
                    Current.Entries.Add(Current.Measurements[x].Type, Current.Measurements[x].StopProfiling(false));
                }
            }
            Current.Running = true;
            for (int x = 0; x < Current.Measurements.Length; ++x)
            {
                Current.Measurements[x].StartProfiling();
            }
        }

        /// <summary>
        /// Disposes the object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>True if they are equal, false otherwise</returns>
        public override bool Equals(object obj)
        {
            var Temp = obj as InternalProfiler;
            return Temp != null && Temp == this;
        }

        /// <summary>
        /// Gets the hash code for the profiler
        /// </summary>
        /// <returns>The hash code</returns>
        public override int GetHashCode()
        {
            return Function.GetHashCode();
        }

        /// <summary>
        /// Gets the value at a specific percentile
        /// </summary>
        /// <param name="percentage">The percentage.</param>
        /// <param name="type">The type.</param>
        /// <returns>The value at a specific percentile</returns>
        public IResultEntry Percentile(decimal percentage, string type)
        {
            if (!Entries.Keys.Contains(type))
                return null;
            var PercentileIndex = (int)(Entries[type].Count() * percentage);
            return Entries[type].OrderBy(x => x.Value).ElementAt(PercentileIndex);
        }

        /// <summary>
        /// Creates a profiler object and starts profiling
        /// </summary>
        /// <param name="functionName">Function name</param>
        /// <returns>An IDisposable that is used to stop profiling</returns>
        public IDisposable Profile(string functionName)
        {
            return new InternalProfiler(
                functionName,
                Canister.Builder.Bootstrapper.ResolveAll<IMeasurement>(),
                Canister.Builder.Bootstrapper?.Resolve<BigBook.Caching.Manager>());
        }

        /// <summary>
        /// Starts profiling
        /// </summary>
        /// <returns>The root profiler</returns>
        public IDisposable StartProfiling()
        {
            return Root;
        }

        /// <summary>
        /// Stops the timer and registers the information
        /// </summary>
        public void Stop()
        {
            if (Current == null)
            {
                return;
            }
            if (Current.Running)
            {
                Current.Running = false;
                for (int x = 0; x < Current.Measurements.Length; ++x)
                {
                    Current.Entries.Add(Current.Measurements[x].Type, Current.Measurements[x].StopProfiling(false));
                }
                Current = Parent;
            }
        }

        /// <summary>
        /// Stops profiling
        /// </summary>
        /// <param name="discardResults">Discard results</param>
        /// <returns>The root profiler</returns>
        public IProfilerResult StopProfiling(bool discardResults)
        {
            if (Root == null)
                return null;
            Root.Stop();
            if (discardResults)
                Root.Entries.Clear();
            return Root;
        }

        /// <summary>
        /// Outputs the information to a table
        /// </summary>
        /// <returns>an html string containing the information</returns>
        public override string ToString()
        {
            var Builder = new StringBuilder();
            Level.Times(_ => Builder.Append("\t"));
            Builder.AppendLineFormat("{0} ({1} ms)", Function, Entries["Time"].Sum(x => x.Value));
            foreach (string Key in Children.Keys)
            {
                Builder.AppendLine(Children[Key].ToString());
            }
            return Builder.ToString();
        }

        /// <summary>
        /// Disposes of the objects
        /// </summary>
        /// <param name="Disposing">
        /// True to dispose of all resources, false only disposes of native resources
        /// </param>
        protected virtual void Dispose(bool Disposing)
        {
            if (Disposing)
                Stop();
        }
    }
}