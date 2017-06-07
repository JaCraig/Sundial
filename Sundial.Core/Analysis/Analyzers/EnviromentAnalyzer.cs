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
using Sundial.Core.Analysis.Enums;
using Sundial.Core.Analysis.Interfaces;
using Sundial.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sundial.Core.Analysis.Analyzers
{
    /// <summary>
    /// Environment analyzer
    /// </summary>
    /// <seealso cref="IAnalyzer"/>
    public class EnviromentAnalyzer : IAnalyzer
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name => "Environment analyzer";

        /// <summary>
        /// Analyzes the specified result.
        /// </summary>
        /// <param name="results">The results.</param>
        /// <returns>The list of findings</returns>
        public IEnumerable<Finding> Analyze(IEnumerable<IResult> results)
        {
            if (results == null || !results.Any())
                return new Finding[0];
            List<Finding> ReturnValues = new List<Finding>();
            foreach (var TempResult in results)
            {
                if (!TempResult.Task.GetType().GetTypeInfo().Assembly.IsJitOptimized())
                {
                    ReturnValues.Add(new Finding($"\"{TempResult.Task.Name}\" was not built with optimization enabled. This is most likely due to it being in debug mode. Build in release mode for more accurate results.", FindingType.Warning));
                }
            }
            return ReturnValues;
        }
    }
}