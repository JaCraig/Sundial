using Sundial.Core.Manager.Default;
using System.Threading;
using Xunit;

namespace Sundial.Core.Tests.Manager.Default
{
    public class StopWatchTests
    {
        [Fact]
        public void BasicTest()
        {
            var TestObject = new StopWatch();
            TestObject.Start();
            Thread.Sleep(100);
            TestObject.Stop();
            Assert.InRange(TestObject.ElapsedTime, 80, 200);
            TestObject.Start();
            Thread.Sleep(100);
            TestObject.Stop();
            Assert.InRange(TestObject.ElapsedTime, 80, 200);
        }
    }
}