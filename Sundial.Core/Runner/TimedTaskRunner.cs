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
using BigBook.ExtensionMethods;
using Sundial.Core.Analysis;
using Sundial.Core.Attributes;
using Sundial.Core.Interfaces;
using Sundial.Core.Manager;
using Sundial.Core.Reports;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Sundial.Core.Runner
{
    /// <summary>
    /// Timed task runner
    /// </summary>
    public class TimedTaskRunner
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimedTaskRunner"/> class.
        /// </summary>
        /// <param name="tasks">The tasks.</param>
        /// <param name="analysisManager">The analysis manager.</param>
        /// <param name="profilerManager">The profiler manager.</param>
        /// <param name="reportManager">The report manager.</param>
        /// <exception cref="System.ArgumentNullException">
        /// reportManager or profilerManager or analysisManager
        /// </exception>
        public TimedTaskRunner(IEnumerable<ITimedTask> tasks, AnalysisManager analysisManager, ProfilerManager profilerManager, ReportManager reportManager)
        {
            ReportManager = reportManager ?? throw new ArgumentNullException(nameof(reportManager));
            ProfilerManager = profilerManager ?? throw new ArgumentNullException(nameof(profilerManager));
            AnalysisManager = analysisManager ?? throw new ArgumentNullException(nameof(analysisManager));
            tasks = tasks ?? new List<ITimedTask>();
            Tasks = new ListMapping<ISeries, ITimedTask>();
            foreach (var Task in tasks)
            {
                var SeriesAttribute = Task.GetType().GetTypeInfo().Attribute<SeriesAttribute>() ?? new SeriesAttribute(Task.Name);
                Tasks.Add(SeriesAttribute, Task);
            }
            Randomizer = new Mirage.Random();
        }

        /// <summary>
        /// Gets the analysis manager.
        /// </summary>
        /// <value>The analysis manager.</value>
        public AnalysisManager AnalysisManager { get; }

        /// <summary>
        /// Gets the profiler manager.
        /// </summary>
        /// <value>The profiler manager.</value>
        public ProfilerManager ProfilerManager { get; }

        /// <summary>
        /// Gets the report manager.
        /// </summary>
        /// <value>The report manager.</value>
        public ReportManager ReportManager { get; }

        /// <summary>
        /// Gets the tasks.
        /// </summary>
        /// <value>The tasks.</value>
        public ListMapping<ISeries, ITimedTask> Tasks { get; }

        /// <summary>
        /// Gets or sets the randomizer.
        /// </summary>
        /// <value>The randomizer.</value>
        private Mirage.Random Randomizer { get; set; }

        /// <summary>
        /// Runs the tasks using the exporter specified.
        /// </summary>
        /// <param name="exporterToUse">The exporter to use.</param>
        /// <returns></returns>
        public IEnumerable<string> Run(string exporterToUse)
        {
            List<string> ReturnValues = new List<string>();
            foreach (var Series in Randomizer.Shuffle(Tasks.Keys))
            {
                List<IResult> Results = new List<IResult>();
                foreach (var Task in Randomizer.Shuffle(Tasks[Series]))
                {
                    ((object)null).Cache("Root_Profiler", "Item");
                    ((object)null).Cache("Current_Profiler", "Item");
                    using (var Profiler = ProfilerManager.StartProfiling())
                    {
                        for (int x = 0; x < Series.Iterations; ++x)
                        {
                            using (var Timer = ProfilerManager.Profile(Task.Name))
                            {
                                Task.Run();
                            }
                        }
                    }
                    var Result = ProfilerManager.StopProfiling(false);
                    var TempResult = new Result(Task, Result.Children[Task.Name]);
                    Results.Add(TempResult);
                }
                var Findings = AnalysisManager.Analyze(Results);
                ReturnValues.Add(ReportManager.Export(exporterToUse, Series, Results, Findings));
            }
            return ReturnValues;
        }
    }
}