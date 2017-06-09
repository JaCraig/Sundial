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
using FileCurator;
using Sundial.Core.Analysis;
using Sundial.Core.Interfaces;
using Sundial.Core.Reports.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sundial.Core.Reports.Exporters
{
    /// <summary>
    /// HTML Exporter
    /// </summary>
    /// <seealso cref="ExporterBaseClass"/>
    public class HTMLExporter : ExporterBaseClass
    {
        private Dictionary<string, string> ScriptFiles = new Dictionary<string, string>()
        {
            ["excanvas.min.js"] = "resource://Sundial.Core/Sundial.Core.Reports.Exporters.Resources.Scripts.excanvas.min.js",
            ["jquery.flot.axislabels.js"] = "resource://Sundial.Core/Sundial.Core.Reports.Exporters.Resources.Scripts.jquery.flot.axislabels.js",
            ["jquery.flot.min.js"] = "resource://Sundial.Core/Sundial.Core.Reports.Exporters.Resources.Scripts.jquery.flot.min.js",
            ["jquery.tablesorter.min.js"] = "resource://Sundial.Core/Sundial.Core.Reports.Exporters.Resources.Scripts.jquery.tablesorter.min.js",
            ["jquery-1.11.2.min.js"] = "resource://Sundial.Core/Sundial.Core.Reports.Exporters.Resources.Scripts.jquery-1.11.2.min.js"
        };

        private Dictionary<string, string> StyleFiles = new Dictionary<string, string>()
        {
            ["Layout.css"] = "resource://Sundial.Core/Sundial.Core.Reports.Exporters.Resources.Styles.Layout.css"
        };

        /// <summary>
        /// Gets the file name suffix.
        /// </summary>
        /// <value>The file name suffix.</value>
        public override string FileNameSuffix => ".html";

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public override string Name => "HTML";

        /// <summary>
        /// Runs this instance.
        /// </summary>
        /// <param name="series">The series to export.</param>
        /// <param name="results">The result.</param>
        /// <param name="findings">The findings.</param>
        /// <returns>The file location.</returns>
        public override string Export(ISeries series, IEnumerable<IResult> results, IEnumerable<Finding> findings)
        {
            CopyResources();
            return WriteResultPage(series, results, findings);
        }

        /// <summary>
        /// Used to write a summary about the various series tested.
        /// </summary>
        /// <param name="summaryData">The summary data.</param>
        /// <returns>The file exported from the system.</returns>
        public override string Summarize(ListMapping<ISeries, IResult> summaryData)
        {
            var DestinationDirectory = new DirectoryInfo($"./FinalReports/");
            FileInfo IndexFile = WriteIndexPage(summaryData, DestinationDirectory);
            return IndexFile.FullName;
        }

        /// <summary>
        /// Gets the memory data.
        /// </summary>
        /// <param name="results">The results.</param>
        /// <returns>The memory data</returns>
        private static string GetMemoryData(IEnumerable<IResult> results)
        {
            int Count = 0;
            return results.ToString(x =>
            {
                Count = 0;
                string Data = @"{
                label: '" + x.Name + @"',
                data: [" + x.Values["Memory"].ToString(y => "[" + (++Count).ToString() + "," + y.Value + "]", ",") + @"],
                }";
                return Data;
            }, ",");
        }

        /// <summary>
        /// Gets the item at a specific percentile formatted as a string for Flot.
        /// </summary>
        /// <param name="results">The results.</param>
        /// <param name="percentile">The percentile.</param>
        /// <returns>The string formatted for flot</returns>
        private static string GetPercentile(IEnumerable<IResult> results, decimal percentile)
        {
            int Count = 0;
            return results.ToString(x => $"[{(++Count).ToString()},{x.Percentile("Time", percentile).Value.ToString()}]", ",");
        }

        /// <summary>
        /// Gets the table data.
        /// </summary>
        /// <param name="results">The results.</param>
        /// <returns>The table data</returns>
        private static string GetTableData(IEnumerable<IResult> results)
        {
            return results.ToString(x => string.Format("<tr><td>{0}</td><td>{1:0.##}ms</td><td>{2:0.##}ms</td><td>{3:0.##}ms</td><td>{4:0.##}ms</td><td>{5:0.##}ms</td><td>{6:0.##}ms</td><td>{7:0.##}</td></tr>",
                    x.Name,
                    x.Values["Time"].Average(y => y.Value),
                    x.Values["Time"].Select(y => (double)y.Value).StandardDeviation(),
                    x.Values["Time"].Min(y => y.Value),
                    x.Percentile("Time", 0.90m).Value,
                    x.Percentile("Time", 0.99m).Value,
                    x.Values["Time"].Max(y => y.Value),
                    1000.0m / x.Values["Time"].Average(y => y.Value)),
                "");
        }

        /// <summary>
        /// Gets the time ticks used by flot.
        /// </summary>
        /// <param name="results">The results.</param>
        /// <returns>The string formatted for flot</returns>
        private static string GetTimeTicks(IEnumerable<IResult> results)
        {
            int Count = 0;
            return results.ToString(x => $"[{(++Count).ToString()},\"{x.Name}\"]", ",");
        }

        private void CopyResources()
        {
            var DestinationDirectory = new DirectoryInfo($"./FinalReports/");
            var ScriptsDirectory = new DirectoryInfo(DestinationDirectory.FullName + "Scripts");
            var StylesDirectory = new DirectoryInfo(DestinationDirectory.FullName + "Styles");
            foreach (var Script in ScriptFiles)
            {
                string Data = new FileInfo(Script.Value);
                new FileInfo(ScriptsDirectory.FullName + "/" + Script.Key).Write(Data);
            }
            foreach (var Style in StyleFiles)
            {
                string Data = new FileInfo(Style.Value);
                new FileInfo(StylesDirectory.FullName + "/" + Style.Key).Write(Data);
            }
        }

        /// <summary>
        /// Gets the series list.
        /// </summary>
        /// <param name="results">The results.</param>
        /// <returns>The list as a string</returns>
        private string GetSeriesList(ListMapping<ISeries, IResult> results)
        {
            return new StringBuilder().Append("<ul>")
                                        .Append(results.Where(x => x.Key.Exporters.Contains("HTML"))
                                                       .ToString(x => "<li><a href=\"" +
                                                                    x.Key.Name +
                                                                    "Result.html\">" +
                                                                    (string.IsNullOrEmpty(x.Key.Name) ? "Results" : x.Key.Name) +
                                                                    "</a></li>",
                                                                ""))
                                        .Append("</ul>")
                                        .ToString();
        }

        private FileInfo WriteIndexPage(ListMapping<ISeries, IResult> summaryData, DirectoryInfo DestinationDirectory)
        {
            var Result = new FileInfo("resource://Sundial.Core/Sundial.Core.Reports.Exporters.Resources.Index.html").Read();
            var IndexFile = new FileInfo(System.IO.Path.Combine(DestinationDirectory.FullName, "Index.html"));
            IndexFile.Write(string.Format(Result,
                                    GetSeriesList(summaryData),
                                    DateTime.Now.ToString("MM/dd/yyyy")));
            return IndexFile;
        }

        private string WriteResultPage(ISeries series, IEnumerable<IResult> results, IEnumerable<Finding> findings)
        {
            var DestinationDirectory = new DirectoryInfo($"./FinalReports/");
            var ResultText = new FileInfo("resource://Sundial.Core/Sundial.Core.Reports.Exporters.Resources.Results.html").Read();
            var OrderedResults = results.OrderByDescending(x => x.Percentile("Time", 0.95m).Value);
            string FiftyPercentile = GetPercentile(OrderedResults, 0.5m);
            string SeventyFivePercentile = GetPercentile(OrderedResults, 0.75m);
            string NintyPercentile = GetPercentile(OrderedResults, 0.9m);
            string NintyFivePercentile = GetPercentile(OrderedResults, 0.95m);
            string Ticks = GetTimeTicks(OrderedResults);
            string Description = "<ul>" + findings.ToString(x => "<li>" + x.Description + "</li>", "") + "</ul>";
            string MemoryData = GetMemoryData(OrderedResults);
            string Rows = GetTableData(OrderedResults);
            var ResultPage = new FileInfo(System.IO.Path.Combine(DestinationDirectory.FullName, series.Name + "Result.html"));
            ResultPage.Write(string.Format(ResultText,
                                    FiftyPercentile,
                                    SeventyFivePercentile,
                                    NintyPercentile,
                                    NintyFivePercentile,
                                    Ticks,
                                    Rows,
                                    "",
                                    MemoryData,
                                    Description,
                                    string.IsNullOrEmpty(series.Name) ? "" : (series.Name + " "),
                                    DateTime.Now.ToString("MM/dd/yyyy")));
            return ResultPage.FullName;
        }
    }
}