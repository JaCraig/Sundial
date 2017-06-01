using Sundial.Core.ExtensionMethods;
using Sundial.Core.Tests.BaseClasses;
using System;
using System.Threading;
using Xunit;

namespace Sundial.Core.Tests.ExtensionMethods
{
    public class TimerExtensions : TestingDirectoryFixture
    {
        [Fact]
        public void TimeTest()
        {
            new Action(() => Thread.Sleep(100)).Time();
        }
    }
}