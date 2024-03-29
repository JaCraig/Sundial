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

using Canister.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Sundial.Core.Analysis;
using Sundial.Core.Analysis.Interfaces;
using Sundial.Core.Interfaces;
using Sundial.Core.Manager;
using Sundial.Core.Manager.Default;
using Sundial.Core.Manager.Interfaces;
using Sundial.Core.Reports;
using Sundial.Core.Reports.Interfaces;
using Sundial.Core.Runner;

namespace Sundial.Core.Module
{
    /// <summary>
    /// Sundial module
    /// </summary>
    /// <seealso cref="IModule"/>
    public class SundialModule : IModule
    {
        /// <summary>
        /// Order to run it in
        /// </summary>
        public int Order => 0;

        /// <summary>
        /// Loads the module
        /// </summary>
        /// <param name="bootstrapper">Bootstrapper to register with</param>
        public void Load(IServiceCollection bootstrapper)
        {
            if (bootstrapper == null)
                return;
            bootstrapper.AddAllTransient<IProfiler>();
            bootstrapper.AddAllTransient<IMeasurement>();
            bootstrapper.AddTransient<StopWatch>();
            bootstrapper.AddAllTransient<IAnalyzer>();
            bootstrapper.AddAllTransient<IExporter>();
            bootstrapper.AddAllTransient<ITimedTask>();
            bootstrapper.AddSingleton<AnalysisManager>();
            bootstrapper.AddSingleton<ProfilerManager>();
            bootstrapper.AddSingleton<ReportManager>();
            bootstrapper.AddSingleton<TimedTaskRunner>();
        }
    }
}