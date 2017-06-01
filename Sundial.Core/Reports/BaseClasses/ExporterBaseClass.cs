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

using Sundial.Core.Interfaces;
using Sundial.Core.Reports.Interfaces;
using System.Collections.Generic;

namespace Sundial.Core.Reports.BaseClasses
{
    /// <summary>
    /// Exporter base class
    /// </summary>
    /// <seealso cref="IExporter"/>
    public abstract class ExporterBaseClass : IExporter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExporterBaseClass"/> class.
        /// </summary>
        protected ExporterBaseClass()
        {
        }

        /// <summary>
        /// Gets the file name suffix.
        /// </summary>
        /// <value>The file name suffix.</value>
        public abstract string FileNameSuffix { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public abstract string Name { get; }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        /// <param name="series">The series to export.</param>
        /// <param name="results">The results.</param>
        /// <returns>The file location.</returns>
        public abstract string Export(ISeries series, IEnumerable<IResult> results);
    }
}