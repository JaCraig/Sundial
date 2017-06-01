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
    public class MemoryVariability : IAnalyzer
    {
        private readonly double EPSILON;

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name => "Memory variability analyzer";

        /// <summary>
        /// Analyzes the specified result.
        /// </summary>
        /// <param name="results">The results.</param>
        /// <returns>The list of findings</returns>
        public IEnumerable<Finding> Analyze(IEnumerable<IResult> results)
        {
            var MemoryUsageLeastVariable = results.First(x => System.Math.Abs(results.Min(y => y.Values["Memory"].Select(z => (double)z.Value).StandardDeviation()) - x.Values["Memory"].Select(z => (double)z.Value).StandardDeviation()) < EPSILON);
            return new Finding[] { new Finding($"{MemoryUsageLeastVariable.Name} had the least amount of variability in the memory usage.") };
        }
    }
}