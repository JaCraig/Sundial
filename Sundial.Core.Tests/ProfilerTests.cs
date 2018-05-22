using Sundial.Core.Tests.BaseClasses;
using Xunit;

namespace Sundial.Core.Tests
{
    public class ProfilerTests : TestingDirectoryFixture
    {
        [Fact]
        public void Creation()
        {
            var TestObject = new Profiler("TestName");
            Assert.NotNull(TestObject);
        }

        [Fact]
        public void DisposeTest()
        {
            var TestObject = new Profiler("TestName");
            TestObject.Dispose();
        }

        [Fact]
        public void StartProfiling()
        {
            using (var TestObject = Profiler.StartProfiling())
            {
                Assert.NotNull(TestObject);
            }
        }

        [Fact]
        public void StopProfiling()
        {
            using (var TestObject = Profiler.StartProfiling())
            {
            }
            var Result = Profiler.StopProfiling(false);
            Assert.NotNull(Result);
            Assert.Equal(0, Result.Children.Count);
            Assert.Equal(2, Result.Entries.Count);
            Assert.Equal("Start", Result.Function);
        }

        [Fact]
        public void StopProfilingDiscardResults()
        {
            using (var TestObject = Profiler.StartProfiling())
            {
            }
            var Result = Profiler.StopProfiling(true);
            Assert.NotNull(Result);
            Assert.Equal(0, Result.Children.Count);
            Assert.Empty(Result.Entries);
            Assert.Equal("Start", Result.Function);
        }
    }
}