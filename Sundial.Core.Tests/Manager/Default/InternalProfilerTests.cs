using Microsoft.Extensions.DependencyInjection;
using Sundial.Core.Manager.Default;
using Sundial.Core.Manager.Interfaces;
using Sundial.Core.Tests.BaseClasses;
using System.Linq;
using System.Threading;
using Xunit;

namespace Sundial.Core.Tests.Manager.Default
{
    public class InternalProfilerTests : TestingDirectoryFixture
    {
        [Fact]
        public void BasicTest()
        {
            var TestObject = new InternalProfiler("Func1", GetServiceProvider().GetServices<IMeasurement>(), CacheManager);
            Thread.Sleep(600);
            var A = new InternalProfiler("A", GetServiceProvider().GetServices<IMeasurement>(), CacheManager);
            Thread.Sleep(600);
            A.Stop();
            TestObject.Stop();
            Assert.InRange(A.Entries["Time"].Sum(x => x.Value), 0, 1000);
            Assert.InRange(TestObject.Entries["Time"].Sum(x => x.Value), 0, 1500);
            Assert.InRange(A.Entries["Memory"].Sum(x => x.Value), 0, 100);
            Assert.InRange(A.Entries["CPU"].Sum(x => x.Value), 0, 100);
        }

        [Fact]
        public void Percentile()
        {
            var TestObject = new InternalProfiler("Func1", GetServiceProvider().GetServices<IMeasurement>(), CacheManager);
            TestObject.Entries.Add("A", new Entry(1));
            TestObject.Entries.Add("A", new Entry(2));
            TestObject.Entries.Add("A", new Entry(3));
            TestObject.Entries.Add("A", new Entry(4));
            TestObject.Entries.Add("A", new Entry(5));
            var Result = TestObject.Percentile(.5m, "A");
            Assert.Equal(3, Result.Value);
        }
    }
}