using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Profiler.Manager.Interfaces;
using Xunit;

namespace Sundial.Core.Tests
{
    public class Result
    {
        [Fact]
        public void Creation()
        {
            var TempObject = new Sundial.Core.Result(new List<IResultEntry>(), "Test 1");
            Assert.Equal("Test 1", TempObject.Name);
            Assert.Equal(0, TempObject.Times.Count());
        }
    }
}