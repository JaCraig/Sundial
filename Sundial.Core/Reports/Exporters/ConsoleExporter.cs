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
using Sundial.Core.Analysis;
using Sundial.Core.Interfaces;
using Sundial.Core.Reports.BaseClasses;
using System;
using System.Collections.Generic;

namespace Sundial.Core.Reports.Exporters
{
    /// <summary>
    /// Console exporter
    /// </summary>
    /// <seealso cref="ExporterBaseClass"/>
    public class ConsoleExporter : ExporterBaseClass
    {
        /// <summary>
        /// Gets the file name suffix.
        /// </summary>
        /// <value>The file name suffix.</value>
        public override string FileNameSuffix => "";

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public override string Name => "Console";

        /// <summary>
        /// Runs this instance.
        /// </summary>
        /// <param name="series">The series to export.</param>
        /// <param name="results">The result.</param>
        /// <param name="findings">The findings.</param>
        /// <returns>The various file locations.</returns>
        public override string Export(ISeries series, IEnumerable<IResult> results, IEnumerable<Finding> findings)
        {
            Console.WriteLine("Series: " + series.Name);
            var Result = results.ToString(x => "\t" + x, "\n");
            Console.WriteLine(Result);
            Console.WriteLine();
            Console.WriteLine(findings.ToString(x => x.Type.ToString() + ": " + x.Description, "\n"));
            return "";
        }
    }
}