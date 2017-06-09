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

namespace Sundial.Core.Analysis
{
    /// <summary>
    /// Analysis manager
    /// </summary>
    public class AnalysisManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnalysisManager"/> class.
        /// </summary>
        /// <param name="analyzers">The analyzers.</param>
        /// <exception cref="System.ArgumentNullException">analyzers</exception>
        public AnalysisManager(IEnumerable<IAnalyzer> analyzers)
        {
            Analyzers = analyzers ?? new List<IAnalyzer>();
        }

        /// <summary>
        /// Gets the analyzers.
        /// </summary>
        /// <value>The analyzers.</value>
        public IEnumerable<IAnalyzer> Analyzers { get; }

        /// <summary>
        /// Analyzes the specified result.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>The findings found by the system.</returns>
        public IEnumerable<Finding> Analyze(IEnumerable<IResult> result)
        {
            List<Finding> Results = new List<Finding>();
            foreach (var Analyzer in Analyzers)
            {
                Results.AddRange(Analyzer.Analyze(result));
            }
            return Results;
        }
    }
}