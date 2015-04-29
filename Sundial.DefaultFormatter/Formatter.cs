/*
Copyright (c) 2015 <a href="http://www.gutgames.com">James Craig</a>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.*/

using Sundial.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.DataTypes;
using Utilities.IO;

namespace Sundial.DefaultFormatter
{
    /// <summary>
    /// Default formatter
    /// </summary>
    public class Formatter : IDataFormatter
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get { return "Default formatter"; } }

        /// <summary>
        /// Formats the specified results.
        /// </summary>
        /// <param name="Results">The results.</param>
        /// <param name="OutputDirectory">The output directory.</param>
        public void Format(IEnumerable<Core.Result> Results, string OutputDirectory)
        {
            Results = Results.OrderByDescending(x => x.Percentile(0.95m).Time);
            string Result = new FileInfo("resource://Sundial.DefaultFormatter/Sundial.DefaultFormatter.Results.html").Read();
            string FiftyPercentile = GetPercentile(Results, 0.5m);
            string SeventyFivePercentile = GetPercentile(Results, 0.75m);
            string NintyPercentile = GetPercentile(Results, 0.9m);
            string NintyFivePercentile = GetPercentile(Results, 0.95m);
            string Ticks = GetTimeTicks(Results);
            string Description = GetDescription(Results);
            string CPUData = GetCPUData(Results);
            string MemoryData = GetMemoryData(Results);
            string Rows = GetTableData(Results);
            new FileInfo(System.IO.Path.Combine(OutputDirectory, "Result.html"))
                .Write(string.Format(Result,
                                        FiftyPercentile,
                                        SeventyFivePercentile,
                                        NintyPercentile,
                                        NintyFivePercentile,
                                        Ticks,
                                        Rows,
                                        CPUData,
                                        MemoryData,
                                        Description));
            Dictionary<string, string> Scripts = new Dictionary<string, string>();
            Scripts.Add("excanvas.min.js", "resource://Sundial.DefaultFormatter/Sundial.DefaultFormatter.Scripts.excanvas.min.js");
            Scripts.Add("jquery-1.11.2.min.js", "resource://Sundial.DefaultFormatter/Sundial.DefaultFormatter.Scripts.jquery-1.11.2.min.js");
            Scripts.Add("jquery.flot.axislabels.js", "resource://Sundial.DefaultFormatter/Sundial.DefaultFormatter.Scripts.jquery.flot.axislabels.js");
            Scripts.Add("jquery.flot.min.js", "resource://Sundial.DefaultFormatter/Sundial.DefaultFormatter.Scripts.jquery.flot.min.js");
            Scripts.Add("jquery.tablesorter.min.js", "resource://Sundial.DefaultFormatter/Sundial.DefaultFormatter.Scripts.jquery.tablesorter.min.js");
            Scripts.ForEach(x =>
            {
                string Data = new FileInfo(x.Value);
                new FileInfo(System.IO.Path.Combine(OutputDirectory + "\\Scripts", x.Key)).Write(Data);
            });
            Result = new FileInfo("resource://Sundial.DefaultFormatter/Sundial.DefaultFormatter.Styles.Layout.css").Read();
            new FileInfo(System.IO.Path.Combine(OutputDirectory + "\\Styles", "Layout.css")).Write(Result);
        }

        /// <summary>
        /// Gets the table data.
        /// </summary>
        /// <param name="Results">The results.</param>
        /// <returns>The table data</returns>
        private static string GetTableData(IEnumerable<Core.Result> Results)
        {
            return Results.ToString(x => "<tr><td>" + x.Name + "</td><td>" + x.Times.Average(y => y.Time).ToString("0.##") + "ms</td><td>" + x.Times.Select(y => (double)y.Time).StandardDeviation().ToString("0.##") + "ms</td><td>" + x.Times.Min(y => y.Time).ToString("0.##") + "ms</td><td>" + x.Percentile(0.90m).Time.ToString("0.##") + "ms</td><td>" + x.Percentile(0.99m).Time.ToString("0.##") + "ms</td><td>" + x.Times.Max(y => y.Time).ToString("0.##") + "ms</td><td>" + (1000.0d / x.Times.Average(y => y.Time)).ToString("0.##") + "</td></tr>", "");
        }

        /// <summary>
        /// Gets the memory data.
        /// </summary>
        /// <param name="Results">The results.</param>
        /// <returns>The memory data</returns>
        private static string GetMemoryData(IEnumerable<Core.Result> Results)
        {
            int Count = 0;
            return Results.ToString(x =>
            {
                Count = 0;
                string Data = @"{
                label: '" + x.Name + @"',
                data: [" + x.Times.ToString(y => "[" + (++Count).ToString() + "," + y.Memory + "]", ",") + @"],
                }";
                return Data;
            }, ",");
        }

        /// <summary>
        /// Gets the cpu data.
        /// </summary>
        /// <param name="Results">The results.</param>
        /// <returns>The CPU data</returns>
        private static string GetCPUData(IEnumerable<Core.Result> Results)
        {
            int Count = 0;
            return Results.ToString(x =>
            {
                Count = 0;
                string Data = @"{
                label: '" + x.Name + @"',
                data: [" + x.Times.ToString(y => "[" + (++Count).ToString() + "," + y.CPUUsage + "]", ",") + @"],
                }";
                return Data;
            }, ",");
        }

        /// <summary>
        /// Gets the time ticks used by flot.
        /// </summary>
        /// <param name="Results">The results.</param>
        /// <returns>The string formatted for flot</returns>
        private static string GetTimeTicks(IEnumerable<Core.Result> Results)
        {
            int Count = 0;
            return Results.ToString(x => "[" + (++Count).ToString() + ",\"" + x.Name + "\"]", ",");
        }

        /// <summary>
        /// Gets the item at a specific percentile formatted as a string for Flot.
        /// </summary>
        /// <param name="Results">The results.</param>
        /// <param name="Percentile">The percentile.</param>
        /// <returns>The string formatted for flot</returns>
        private static string GetPercentile(IEnumerable<Core.Result> Results, decimal Percentile)
        {
            int Count = 0;
            return Results.ToString(x => "[" + (++Count).ToString() + "," + x.Percentile(Percentile).Time.ToString() + "]", ",");
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <param name="Results">The results.</param>
        /// <returns>The description</returns>
        private string GetDescription(IEnumerable<Core.Result> Results)
        {
            StringBuilder Builder = new StringBuilder();
            var AverageResult = Results.First(x => Results.Min(y => y.Percentile(0.5m).Time) == x.Percentile(0.5m).Time);
            var Min95Result = Results.First(x => Results.Min(y => y.Percentile(0.95m).Time) == x.Percentile(0.95m).Time);
            Builder.AppendFormat("<p>The test results below were run on {0}. The following items were tested:</p><ul>{1}</ul><p>The results themselves are not 100% accurate as things such as garbage collection, background processes, etc. can effect the outcome. As such these should only be used as a guideline and more precise tools should be used to figure out any performance issues. That said, the following points of interest were discovered:</p>", DateTime.Now.ToString("MMMM dd, yyyy HH:mm:ss tt"), Results.ToString(x => "<li>" + x.Name + "</li>", ""));
            if (AverageResult.Name != Min95Result.Name)
                Builder.AppendFormat("<p>\"{0}\" on average is faster but in the 95% instances, we see \"{1}\" showing better in the worst case scenarios.</p>", AverageResult.Name, Min95Result.Name);
            else
                Builder.AppendFormat("<p>\"{0}\" is consistantly the fastest item in the group.</p>", AverageResult.Name);
            var MemoryUsageMin = Results.First(x => Results.Min(y => y.Times.Average(z => z.Memory)) == x.Times.Average(z => z.Memory));
            var MemoryUsageLeastVariable = Results.First(x => Results.Min(y => y.Times.Select(z => (double)z.Memory).StandardDeviation()) == x.Times.Select(z => (double)z.Memory).StandardDeviation());
            Builder.AppendFormat("<p>On average \"{0}\" used the least amount of memory throughout the test's lifecycle. {1} had the least amount of variability in the memory usage. This however is not 100% accurate as garbage collection may kick in at odd times, memory leaks may have occurred that pushed other item's values higher, etc.</p>", MemoryUsageMin.Name, MemoryUsageMin.Name == MemoryUsageLeastVariable.Name ? ("\"" + MemoryUsageLeastVariable.Name + "\" also ") : ("Where as \"" + MemoryUsageLeastVariable.Name + "\""));
            var CPUUsageMin = Results.First(x => Results.Min(y => y.Times.Average(z => z.CPUUsage)) == x.Times.Average(z => z.CPUUsage));
            var CPUUsageLeastVariable = Results.First(x => Results.Min(y => y.Times.Select(z => (double)z.CPUUsage).StandardDeviation()) == x.Times.Select(z => (double)z.CPUUsage).StandardDeviation());
            Builder.AppendFormat("<p>On average \"{0}\" was the least taxing on the CPU. {1} had the least amount of variability. Once again, not 100% accurate due to the rate of sampling, etc.</p>", CPUUsageMin.Name, CPUUsageMin.Name == CPUUsageLeastVariable.Name ? ("\"" + CPUUsageLeastVariable.Name + "\" also ") : ("Where as \"" + CPUUsageLeastVariable.Name + "\""));
            return Builder.ToString();
        }
    }
}