using SerializationExample.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.IoC.Interfaces;
using Utilities.Random;

namespace SerializationExample.Modules
{
    public class DataModule : IModule
    {
        /// <summary>
        /// Order to run this in
        /// </summary>
        public int Order { get { return 10; } }

        /// <summary>
        /// Loads the module using the bootstrapper
        /// </summary>
        /// <param name="Bootstrapper">Bootstrapper used to register various objects</param>
        public void Load(IBootstrapper Bootstrapper)
        {
            if (Bootstrapper == null)
                return;
            var TempRand = new System.Random();
            Bootstrapper.Register<TestObject>(() => TempRand.NextClass<TestObject>());
            Bootstrapper.Register<string>(() => @"{ ""BoolReference"" : true,
  ""ByteArrayReference"" : [ 1,
      2,
      3,
      4
    ],
  ""ByteReference"" : 200,
  ""CharReference"" : ""A"",
  ""DecimalReference"" : 1.234,
  ""DoubleReference"" : 1.234,
  ""FloatReference"" : 1.234,
  ""GuidReference"" : ""5bec9017-7c9e-4c52-a8d8-ac511c464370"",
  ""ID"" : 55,
  ""IntReference"" : 123,
  ""LongReference"" : 42134123,
  ""NullStringReference"" : null,
  ""ShortReference"" : 1234,
  ""StringReference"" : ""This is a test string""
}");
        }
    }
}