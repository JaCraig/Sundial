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
using Sundial.Core.Interfaces;
using Sundial.Core.Manager.Default;
using Sundial.Core.Manager.Interfaces;
using System;
using System.Linq;

namespace Sundial.Core
{
    /// <summary>
    /// Result for a specific series.
    /// </summary>
    /// <seealso cref="IResult"/>
    public class Result : IResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="profilerResult">The profiler result.</param>
        /// <exception cref="System.ArgumentNullException">profilerResult or task</exception>
        public Result(ITimedTask task, IProfilerResult profilerResult)
        {
            if (profilerResult == null)
                throw new ArgumentNullException(nameof(profilerResult));
            Task = task ?? throw new ArgumentNullException(nameof(task));
            Name = profilerResult.Function ?? "";
            Values = profilerResult.Entries ?? new ListMapping<string, IResultEntry>();
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; }

        /// <summary>
        /// Gets the task.
        /// </summary>
        /// <value>The task.</value>
        public ITimedTask Task { get; }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <value>The values.</value>
        public ListMapping<string, IResultEntry> Values { get; }

        /// <summary>
        /// Percentiles the specified name.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="percentage">The percentage.</param>
        /// <returns>The result entry</returns>
        public IResultEntry Percentile(string type, decimal percentage)
        {
            if (!Values.Keys.Contains(type))
                return new Entry(0);
            var PercentileIndex = (int)(Values[type].Count() * percentage);
            return Values[type].OrderBy(x => x.Value).ElementAt(PercentileIndex);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            if (!Values.ContainsKey("Time") || !Values["Time"].Any())
                return $"{Name} (0 ms)";
            return $"{Name} ({Values["Time"].Average(x => x.Value)} ms)";
        }
    }
}