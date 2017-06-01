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

using Canister.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Sundial.Core.Analysis;
using Sundial.Core.Analysis.Interfaces;
using Sundial.Core.Manager;
using Sundial.Core.Manager.Default;
using Sundial.Core.Manager.Interfaces;
using Sundial.Core.Reports;
using Sundial.Core.Reports.Interfaces;

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
        public void Load(IBootstrapper bootstrapper)
        {
            if (bootstrapper == null)
                return;
            bootstrapper.RegisterAll<IProfiler>();
            bootstrapper.RegisterAll<IMeasurement>();
            bootstrapper.Register<StopWatch>();
            bootstrapper.RegisterAll<IAnalyzer>();
            bootstrapper.RegisterAll<IExporter>();
            bootstrapper.Register<AnalysisManager>(ServiceLifetime.Singleton);
            bootstrapper.Register<ProfilerManager>(ServiceLifetime.Singleton);
            bootstrapper.Register<ReportManager>(ServiceLifetime.Singleton);
        }
    }
}