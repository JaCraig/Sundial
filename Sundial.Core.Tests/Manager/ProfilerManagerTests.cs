using Sundial.Core.Manager;
using Sundial.Core.Manager.Default;
using Sundial.Core.Manager.Default.Measurements;
using Sundial.Core.Manager.Interfaces;
using Sundial.Core.Tests.BaseClasses;
using Xunit;

namespace Sundial.Core.Tests.Manager
{
    public class ProfilerManagerTests : TestingDirectoryFixture
    {
        [Fact]
        public void Creation()
        {
            var TestObject = new ProfilerManager(new IProfiler[] { new InternalProfiler(new IMeasurement[] { new TimeMeasurement() }, CacheManager) });
            Assert.NotNull(TestObject);
        }

        [Fact]
        public void Profile()
        {
            var TestObject = new ProfilerManager(new IProfiler[] { new InternalProfiler(new IMeasurement[] { new TimeMeasurement(), new MemoryMeasurement() }, CacheManager) });
            using (var Profiler = TestObject.StartProfiling())
            {
                using (var TestProfiler = TestObject.Profile("TestProfiler"))
                {
                }
            }
            var Result = TestObject.StopProfiling(false);
            Assert.NotNull(Result);
            Assert.Equal(1, Result.Children.Count);
            Assert.Equal(2, Result.Entries.Count);
            Assert.Equal("Start", Result.Function);
        }

        [Fact]
        public void StartProfiling()
        {
            var TestObject = new ProfilerManager(new IProfiler[] { new InternalProfiler(new IMeasurement[] { new TimeMeasurement(), new MemoryMeasurement() }, CacheManager) });
            using (var Profiler = TestObject.StartProfiling())
            {
                Assert.NotNull(Profiler);
            }
            var Result = TestObject.StopProfiling(false);
        }

        [Fact]
        public void StopProfiling()
        {
            var TestObject = new ProfilerManager(new IProfiler[] { new InternalProfiler(new IMeasurement[] { new TimeMeasurement(), new MemoryMeasurement() }, CacheManager) });
            using (var Profiler = TestObject.StartProfiling())
            {
            }
            var Result = TestObject.StopProfiling(false);
            Assert.NotNull(Result);
            Assert.Equal(0, Result.Children.Count);
            Assert.Equal(2, Result.Entries.Count);
            Assert.Equal("Start", Result.Function);
        }
    }
}