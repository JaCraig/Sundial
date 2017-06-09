using Sundial.Core.Manager.Default.Measurements;
using Xunit;

namespace Sundial.Core.Tests.Manager.Default.Measurements
{
    public class MemoryMeasurementTests
    {
        [Fact]
        public void Profiling()
        {
            var TestObject = new MemoryMeasurement();
            Assert.Equal(TestObject, TestObject.StartProfiling());
            var Result = TestObject.StopProfiling(false);
            Assert.InRange(Result.Value, 0, 100);
        }

        [Fact]
        public void ProfilingDiscard()
        {
            var TestObject = new MemoryMeasurement();
            var Result = TestObject.StopProfiling(true);
            Assert.Equal(0, Result.Value);
        }
    }
}