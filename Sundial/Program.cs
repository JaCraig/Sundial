using Sundial.Config;
using Sundial.Core;
using Sundial.Core.Interfaces;
using System;
using System.Collections.Generic;
using Utilities.DataTypes;
using Utilities.Profiler.Manager;
using Utilities.Random;

namespace Sundial
{
    public class Program
    {
        public static void Main(string[] args)
        {
            System.Random TempRand = new System.Random();
            Configuration Config = new Configuration(args);
            using (var Bootstrapper = Utilities.IoC.Manager.Bootstrapper)
            {
                IEnumerable<IDataFormatter> Formatters = Bootstrapper.ResolveAll<IDataFormatter>();
                IEnumerable<ITimedTask> TimedTasks = TempRand.Shuffle(Bootstrapper.ResolveAll<ITimedTask>());

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
                IEnumerable<Result> Results = Result.Children.ForEach(x => new Result(x.Value.Times, x.Key));
                Formatters.ForEach(x => x.Format(Results, Config.OutputDirectory));
            }
        }
    }
}