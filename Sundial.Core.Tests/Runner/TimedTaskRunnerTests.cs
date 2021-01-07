using Microsoft.Extensions.Logging;
using Sundial.Core.Analysis.Interfaces;
using Sundial.Core.Interfaces;
using Sundial.Core.Manager.Interfaces;
using Sundial.Core.Reports.Interfaces;
using Sundial.Core.Runner;
using Sundial.Core.Tests.BaseClasses;
using System.Collections.Generic;
using Xunit;

namespace Sundial.Core.Tests.Runner
{
    public class TimedTaskRunnerTests : TestingDirectoryFixture
    {
        [Fact]
        public void CanisterCreation()
        {
            var TestObject = Canister.Builder.Bootstrapper.Resolve<TimedTaskRunner>();
            Assert.NotNull(TestObject.AnalysisManager);
            Assert.NotNull(TestObject.ProfilerManager);
            Assert.NotNull(TestObject.ReportManager);
            Assert.NotNull(TestObject.Tasks);
            Assert.NotEmpty(TestObject.Tasks);
        }

        [Fact]
        public void Creation()
        {
            var TestObject = new TimedTaskRunner(new List<ITimedTask>(),
                new Core.Analysis.AnalysisManager(new List<IAnalyzer>()),
                new Core.Manager.ProfilerManager(new List<IProfiler>()),
                new Core.Reports.ReportManager(new List<IExporter>()),
                Canister.Builder.Bootstrapper.Resolve<ILogger<TimedTaskRunner>>());
            Assert.NotNull(TestObject.AnalysisManager);
            Assert.NotNull(TestObject.ProfilerManager);
            Assert.NotNull(TestObject.ReportManager);
            Assert.NotNull(TestObject.Tasks);
            Assert.Empty(TestObject.Tasks);
        }

        [Fact]
        public void Run()
        {
            var TestObject = Canister.Builder.Bootstrapper.Resolve<TimedTaskRunner>();
            var Result = TestObject.Run();
            Assert.NotEmpty(Result);
        }
    }
}