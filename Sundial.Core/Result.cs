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

using System.Collections.Generic;
using System.Linq;
using Utilities.DataTypes;
using Utilities.Profiler.Manager.Default;
using Utilities.Profiler.Manager.Interfaces;

namespace Sundial.Core
{
    /// <summary>
    /// Data results
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class.
        /// </summary>
        /// <param name="times">The times.</param>
        /// <param name="name">The name.</param>
        public Result(IEnumerable<IResultEntry> times, string name)
        {
            this.Times = times.ToList(x => x) ?? new List<IResultEntry>();
            this.Name = string.IsNullOrEmpty(name) ? "" : name;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the times.
        /// </summary>
        /// <value>The times.</value>
        public IEnumerable<IResultEntry> Times { get; private set; }

        /// <summary>
        /// Gets the value at a specific percentile
        /// </summary>
        /// <param name="Percentage">The percentage.</param>
        /// <returns>The value at a specific percentile</returns>
        public IResultEntry Percentile(decimal Percentage)
        {
            int PercentileIndex = (int)(Times.Count() * Percentage);
            return Times.OrderBy(x => x.Time).ElementAt(PercentileIndex);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return Name + ": " + Times.Average(x => x.Time);
        }
    }
}