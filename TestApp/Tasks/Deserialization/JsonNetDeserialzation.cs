using Newtonsoft.Json;
using Sundial.Core.Attributes;
using Sundial.Core.Interfaces;
using TestApp.Tasks.Data;

namespace TestApp.Tasks.Deserialization
{
    [Series("Deserialization", 1000, "Console", "HTML")]
    public class JsonNetDeserialization : ITimedTask
    {
        private const string Data = @"{ ""BoolReference"" : true,
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
}";

        public bool Baseline => true;

        public string Name => "Json.Net";

        public void Dispose()
        {
        }

        public void Run()
        {
            JsonConvert.DeserializeObject(Data, typeof(TestObject), new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.IsoDateFormat });
        }
    }
}