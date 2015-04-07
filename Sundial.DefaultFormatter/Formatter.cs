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
using System.IO;
using System.Linq;
using System.Reflection;
using Utilities.DataTypes;

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
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Sundial.DefaultFormatter.Results.html";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                new Utilities.IO.FileInfo(System.IO.Path.Combine(OutputDirectory, "Result.html"))
                        .Write(string.Format(result, Results.ForEach(x => x.Name + ": " + x.Times.Average() + "ms")
                                      .ToString(x => x, "<br />"))
                              );
            }
        }
    }
}