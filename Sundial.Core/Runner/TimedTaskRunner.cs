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
using DragonHoard.Core;
using Microsoft.Extensions.Logging;
using Sundial.Core.Analysis;
using Sundial.Core.Attributes;
using Sundial.Core.Interfaces;
using Sundial.Core.Manager;
using Sundial.Core.Manager.Default;
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
        /// <param name="logger">The logger</param>
        /// <exception cref="System.ArgumentNullException">
        /// reportManager or profilerManager or analysisManager
        /// </exception>
        public TimedTaskRunner(IEnumerable<ITimedTask> tasks, AnalysisManager analysisManager, ProfilerManager profilerManager, ReportManager reportManager, ILogger<TimedTaskRunner> logger = null)
        {
            ReportManager = reportManager ?? throw new ArgumentNullException(nameof(reportManager));
            ProfilerManager = profilerManager ?? throw new ArgumentNullException(nameof(profilerManager));
            AnalysisManager = analysisManager ?? throw new ArgumentNullException(nameof(analysisManager));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            tasks ??= new List<ITimedTask>();
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
        /// Gets the logger.
        /// </summary>
        /// <value>The logger.</value>
        public ILogger<TimedTaskRunner> Logger { get; }

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
        /// Gets the cache manager.
        /// </summary>
        /// <value>The cache manager.</value>
        protected static Cache CacheManager => Canister.Builder.Bootstrapper.Resolve<Cache>();

        /// <summary>
        /// Gets or sets the randomizer.
        /// </summary>
        /// <value>The randomizer.</value>
        private Mirage.Random Randomizer { get; }

        /// <summary>
        /// Runs the tasks using the exporter specified.
        /// </summary>
        /// <returns>The list of files (if there are any) generated by the runner.</returns>
        public IEnumerable<string> Run()
        {
            Logger.LogInformation("Starting task runner.");
            List<string> ReturnValues = new List<string>();
            ListMapping<ISeries, IResult> SummaryData = new ListMapping<ISeries, IResult>();
            foreach (var Series in Randomizer.Shuffle(Tasks.Keys))
            {
                Logger.LogInformation("Starting series {Info:l}.", Series.Name);
                List<IResult> Results = new List<IResult>();
                foreach (var Task in Randomizer.Shuffle(Tasks[Series]))
                {
                    Logger.LogInformation("Starting task {Info:l}.", Task.Name);
                    CacheManager.GetOrAddCache("Item")?.Set<InternalProfiler>("Root_Profiler", null);
                    CacheManager.GetOrAddCache("Item")?.Set<InternalProfiler>("Current_Profiler", null);
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
                    SummaryData.Add(Series, TempResult);
                }
                Logger.LogInformation("Analyzing data from series {Info:l}.", Series.Name);
                var Findings = AnalysisManager.Analyze(Results);
                Logger.LogInformation("Exporting data from series {Info:l}.", Series.Name);
                ReturnValues.Add(ReportManager.Export(Series.Exporters, Series, Results, Findings));
            }
            ReturnValues.Add(ReportManager.Summarize(SummaryData));
            return ReturnValues;
        }
    }
}