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

using Sundial.Config;
using Sundial.Core;
using Sundial.Core.Attributes;
using Sundial.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities.DataTypes;
using Utilities.Profiler.Manager;
using Utilities.Random;

namespace Sundial
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var TempRand = new System.Random();
            var Config = new Configuration(args);
            using (var Bootstrapper = Utilities.IoC.Manager.Bootstrapper)
            {
                IEnumerable<IDataFormatter> Formatters = Bootstrapper.ResolveAll<IDataFormatter>();
                if (Formatters.Count() == 0)
                {
                    Console.WriteLine("No formatters found.");
                    return;
                }
                IEnumerable<ITimedTask> TimedTasks = TempRand.Shuffle(Bootstrapper.ResolveAll<ITimedTask>());
                if (TimedTasks.Count() == 0)
                {
                    Console.WriteLine("No tasks found.");
                    return;
                }

                using (var Profiler = Manager.StartProfiling())
                {
                    foreach (ITimedTask Task in TimedTasks)
                    {
                        for (int x = 0; x < Config.NumberIterations; ++x)
                        {
                            using (var Timer = Manager.Profile(Task.Name))
                            {
                                Task.Run();
                            }
                        }
                    }
                }
                var Result = Manager.StopProfiling(false);
                var Results = Result.Children
                                    .ForEach(x => new Result(x.Value.Entries, x.Key))
                                    .ToLookup(x => TimedTasks.FirstOrDefault(y => string.Equals(y.Name, x.Name, StringComparison.Ordinal))
                                                             .GetType()
                                                             .Attribute<SeriesAttribute>() as ISeries ?? new SeriesAttribute(""),
                                              x => x);
                Formatters.ForEach(x => x.Format(Results, Config.OutputDirectory));
            }
        }
    }
}