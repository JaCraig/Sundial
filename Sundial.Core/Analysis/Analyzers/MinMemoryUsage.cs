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

using Sundial.Core.Analysis.Interfaces;
using Sundial.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Sundial.Core.Analysis.Analyzers
{
    /// <summary>
    /// Minimum memory usage.
    /// </summary>
    /// <seealso cref="IAnalyzer"/>
    public class MinMemoryUsage : IAnalyzer
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name => "Min memory analyzer";

        /// <summary>
        /// Analyzes the specified result.
        /// </summary>
        /// <param name="results">The results.</param>
        /// <returns>The list of findings</returns>
        public IEnumerable<Finding> Analyze(IEnumerable<IResult> results)
        {
            if (results?.Any() != true)
                return new Finding[0];
            var MemoryUsageMin = results.OrderBy(x => x.Percentile("Memory", 0.5m).Value).First();
            return new Finding[] { new Finding($"On average \"{MemoryUsageMin.Name}\" used the least amount of memory throughout the test's lifecycle.") };
        }
    }
}