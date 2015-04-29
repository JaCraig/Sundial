using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sundial.DefaultFormatter.Tests
{
    public class Formatter
    {
        [Fact]
        public void Creation()
        {
            var TempObject = new Sundial.DefaultFormatter.Formatter();
            Assert.Equal("Default formatter", TempObject.Name);
        }
    }
}