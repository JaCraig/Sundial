using Sundial.Core.Analysis;
using Sundial.Core.Analysis.Analyzers;
using Sundial.Core.Analysis.Interfaces;
using Sundial.Core.Interfaces;
using Sundial.Core.Manager.Default;
using Sundial.Core.Manager.Interfaces;
using Sundial.Core.Tests.BaseClasses;
using Sundial.Core.Tests.MockClasses;
using System.Collections.Generic;
using Xunit;

namespace Sundial.Core.Tests.Analysis
{
    public class AnalysisManagerTests : TestingDirectoryFixture
    {
        [Fact]
        public void Analyze()
        {
            var TestObject = new AnalysisManager(new IAnalyzer[] { new AverageSpeedAnalyzer() });
            Assert.NotEmpty(TestObject.Analyze(new IResult[] { new Result(new TimedTaskMock(), new InternalProfiler(new List<IMeasurement>(), CacheManager)) }));
        }

        [Fact]
        public void Creation()
        {
            var TestObject = new AnalysisManager(new IAnalyzer[] { new AverageSpeedAnalyzer() });
            Assert.Single(TestObject.Analyzers);
        }
    }
}