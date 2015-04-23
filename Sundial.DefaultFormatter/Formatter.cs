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
using System.Collections.Generic;
using System.Linq;
using Utilities.DataTypes;

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
            Results = Results.OrderByDescending(x => x.Percentile(0.95m));
            string Result = new FileInfo("resource://Sundial.DefaultFormatter/Sundial.DefaultFormatter.Results.html").Read();
            int Count = 0;
            string FiftyPercentile = Results.ToString(x => "[" + (++Count).ToString() + "," + x.Percentile(0.5m).ToString() + "]", ",");
            Count = 0;
            string SeventyFivePercentile = Results.ToString(x => "[" + (++Count).ToString() + "," + x.Percentile(0.75m).ToString() + "]", ",");
            Count = 0;
            string NintyPercentile = Results.ToString(x => "[" + (++Count).ToString() + "," + x.Percentile(0.9m).ToString() + "]", ",");
            Count = 0;
            string NintyFivePercentile = Results.ToString(x => "[" + (++Count).ToString() + "," + x.Percentile(0.95m).ToString() + "]", ",");
            Count = 0;
            string Ticks = Results.ToString(x => "[" + (++Count).ToString() + ",\"" + x.Name + "\"]", ",");
            string CPUData = Results.ToString(x => "[" + (++Count).ToString() + ",\"" + x.Name + "\"]", ",");
            string MemoryData = Results.ToString(x => "[" + (++Count).ToString() + ",\"" + x.Name + "\"]", ",");
            string Rows = Results.ToString(x => "<tr><td>" + x.Name + "</td><td>" + x.Times.Average().ToString("0.##") + "ms</td><td>" + x.Times.StandardDeviation().ToString("0.##") + "ms</td><td>" + x.Times.Min().ToString("0.##") + "ms</td><td>" + x.Percentile(0.90m).ToString("0.##") + "ms</td><td>" + x.Percentile(0.99m).ToString("0.##") + "ms</td><td>" + x.Times.Max().ToString("0.##") + "ms</td><td>" + (1000.0d / x.Times.Average()).ToString("0.##") + "</td></tr>", "");
            new FileInfo(System.IO.Path.Combine(OutputDirectory, "Result.html"))
                .Write(string.Format(Result,
                                        FiftyPercentile,
                                        SeventyFivePercentile,
                                        NintyPercentile,
                                        NintyFivePercentile,
                                        Ticks,
                                        Rows,
                                        CPUData,
                                        MemoryData));
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
    }
}