using Sundial.Core.Analysis;
using Sundial.Core.Attributes;
using Sundial.Core.Interfaces;
using Sundial.Core.Manager.Default;
using Sundial.Core.Manager.Interfaces;
using Sundial.Core.Reports;
using Sundial.Core.Reports.Exporters;
using Sundial.Core.Reports.Interfaces;
using Sundial.Core.Tests.BaseClasses;
using Sundial.Core.Tests.MockClasses;
using System.Collections.Generic;
using Xunit;

namespace Sundial.Core.Tests.Reports
{
    public class ReportManagerTests : TestingDirectoryFixture
    {
        [Fact]
        public void Creation()
        {
            var TestObject = new ReportManager(new IExporter[] { new ConsoleExporter() });
            Assert.Equal(1, TestObject.Exporters.Count);
        }

        [Fact]
        public void Export()
        {
            var TestObject = new ReportManager(new IExporter[] { new ConsoleExporter() });
            Assert.Equal("", TestObject.Export(new string[] { "Console" },
                                                new SeriesAttribute("", 1000),
                                                new IResult[] { new Result(new TimedTaskMock(), new InternalProfiler(new List<IMeasurement>())) },
                                                new List<Finding>())[0]);
        }
    }
}