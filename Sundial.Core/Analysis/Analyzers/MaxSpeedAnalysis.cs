﻿/*
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
    /// Max speed analysis
    /// </summary>
    /// <seealso cref="IAnalyzer"/>
    public class MaxSpeedAnalysis : IAnalyzer
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name => "Max speed analysis";

        /// <summary>
        /// Analyzes the specified result.
        /// </summary>
        /// <param name="results">The results.</param>
        /// <returns>The list of findings</returns>
        public IEnumerable<Finding> Analyze(IEnumerable<IResult> results)
        {
            if (results == null || !results.Any())
                return new Finding[0];
            var AverageResult = results.OrderBy(x => x.Percentile("Time", 0.5m).Value).First();
            var Min95Result = results.OrderBy(x => x.Percentile("Time", 0.95m).Value).First();
            if (AverageResult.Name != Min95Result.Name)
                return new Finding[] { new Finding($"\"{AverageResult.Name}\" on average is faster but in the 95% instances, we see \"{Min95Result.Name}\" showing better in the worst case scenarios.") };
            return new Finding[0];
        }
    }
}