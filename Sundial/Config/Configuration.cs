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

using System;
using Utilities.Configuration;
using Utilities.Configuration.Manager.Default;

namespace Sundial.Config
{
    /// <summary>
    /// Configuration class
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public Configuration(string[] args)
        {
            long TempNumberIterations = 0;
            string TempOutputDirectory = "";
            if (args.Length > 0)
            {
                long.TryParse(args[0], out TempNumberIterations);
                NumberIterations = TempNumberIterations;
            }
            else if (!string.IsNullOrEmpty(ConfigurationManager.Get<SystemConfig>().AppSettings["NumberOfIterations"]))
            {
                long.TryParse(ConfigurationManager.Get<SystemConfig>().AppSettings["NumberOfIterations"], out TempNumberIterations);
                NumberIterations = TempNumberIterations;
            }
            else
            {
                Console.WriteLine("Number of iterations:");
                TempNumberIterations = int.Parse(Console.ReadLine());
            }
            if (args.Length > 1)
            {
                TempOutputDirectory = args[1];
            }
            else if (!string.IsNullOrEmpty(ConfigurationManager.Get<SystemConfig>().AppSettings["OutputDirectory"]))
            {
                TempOutputDirectory = ConfigurationManager.Get<SystemConfig>().AppSettings["OutputDirectory"];
            }
            else
            {
                Console.WriteLine("Output directory:");
                TempOutputDirectory = Console.ReadLine();
            }
            this.NumberIterations = TempNumberIterations;
            this.OutputDirectory = TempOutputDirectory;
        }

        /// <summary>
        /// Gets or sets the number iterations.
        /// </summary>
        /// <value>The number iterations.</value>
        public long NumberIterations { get; set; }

        /// <summary>
        /// Gets or sets the output directory.
        /// </summary>
        /// <value>The output directory.</value>
        public string OutputDirectory { get; set; }
    }
}