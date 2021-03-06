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

using BigBook;
using Sundial.Core.Analysis;
using Sundial.Core.Interfaces;
using Sundial.Core.Reports.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sundial.Core.Reports
{
    /// <summary>
    /// Report manager
    /// </summary>
    public class ReportManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportManager"/> class.
        /// </summary>
        /// <param name="exporters">The exporters.</param>
        /// <exception cref="System.ArgumentNullException">exporters</exception>
        public ReportManager(IEnumerable<IExporter> exporters)
        {
            exporters = exporters ?? throw new ArgumentNullException(nameof(exporters));
            Exporters = exporters.ToDictionary(x => x.Name);
        }

        /// <summary>
        /// Gets the exporters.
        /// </summary>
        /// <value>The exporters.</value>
        public IDictionary<string, IExporter> Exporters { get; }

        /// <summary>
        /// Exports the specified exporter to use.
        /// </summary>
        /// <param name="exportersToUse">The exporter to use.</param>
        /// <param name="series">The series.</param>
        /// <param name="results">The result.</param>
        /// <param name="findings">The findings.</param>
        /// <returns>The list of files exported from the application.</returns>
        /// <exception cref="System.ArgumentException">exporterToUse</exception>
        public List<string> Export(string[] exportersToUse, ISeries series, IEnumerable<IResult> results, IEnumerable<Finding> findings)
        {
            if (exportersToUse == null || exportersToUse.Length == 0)
                exportersToUse = new string[] { "Console" };
            List<string> ReturnValue = new List<string>();
            foreach (string exporterToUse in exportersToUse)
            {
                if (!Exporters.ContainsKey(exporterToUse))
                    throw new ArgumentException($"Exporter {exporterToUse} not found");
                ReturnValue.Add(Exporters[exporterToUse].Export(series, results, findings));
            }
            return ReturnValue;
        }

        /// <summary>
        /// Summarizes the specified summary data.
        /// </summary>
        /// <param name="summaryData">The summary data.</param>
        /// <returns>The list of files exported from the application.</returns>
        public List<string> Summarize(ListMapping<ISeries, IResult> summaryData)
        {
            List<string> ReturnValue = new List<string>();
            foreach (var Data in summaryData.Keys)
            {
                var ExportersToUse = Data.Exporters;
                if (ExportersToUse == null || ExportersToUse.Length == 0)
                    ExportersToUse = new string[] { "Console" };
                foreach (string ExporterToUse in ExportersToUse)
                {
                    if (!Exporters.ContainsKey(ExporterToUse))
                        throw new ArgumentException($"Exporter {ExporterToUse} not found");
                    ReturnValue.Add(Exporters[ExporterToUse].Summarize(summaryData));
                }
            }
            return ReturnValue;
        }
    }
}