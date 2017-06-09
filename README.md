# Sundial

[![Build status](https://ci.appveyor.com/api/projects/status/hleskrweq8m91y5q?svg=true)](https://ci.appveyor.com/project/JaCraig/sundial)

Sundial is a simple library that can be used to profile code and gives a nice html output of the results.

## Setting Up the Library

Sundial relies on [Canister](https://github.com/JaCraig/Canister) in order to hook itself up. In order for this to work, you must do the following at startup:

    Canister.Builder.CreateContainer(new List<ServiceDescriptor>())
                    .RegisterSundial()
                    .Build();
					
The RegisterSundial function is an extension method that registers it with the IoC container. When this is done, Sundial is ready to use.

## What does it profile?
Currently it checks speed and memory usage but I plan on opening it up a bit more so that other information can be added/removed depending on your needs.

## Basic Usage

Sundial can be used in one of two ways. The first is simple profiling of code:

    namespace ExampleApp
    {
        internal class Program
        {
            private static void Main(string[] args)
            {
                Canister.Builder.CreateContainer(new List<ServiceDescriptor>())
                        .AddAssembly(typeof(Program).GetTypeInfo().Assembly)
                        .RegisterSundial()
                        .Build();
                using (var TestObject = Profiler.StartProfiling())
                {
                    using (var MyChildProfiler = TestObject.Profile("Description of code block"))
                    {
					    using (var AnotherProfiler = TestObject.Profile("Description of sub code block"))
                        {
                            ...
                        }
                    }
                    using (var MyChildProfiler2 = TestObject.Profile("Description of second code block"))
                    {
                        ...
                    }
                }
                var Result = Profiler.StopProfiling(false);
            }
        }
    }
	
The above example shows how to profile individual blocks of code looking for hotspots. The profiling starts when StartProfiling is called. Each individual block of code is started when TestObject.Profile is called. This function returns an IDisposable. When the object is disposed, the profiler is stopped for that individual section. When StopProfiling is called, it returns an IProfilerResult object. This object will contain all of the results found while profiling.

The other way to use Sundial is to compare two bits of code:

    namespace TestApp
    {
        internal class Program
        {
            private static void Main(string[] args)
            {
                Canister.Builder.CreateContainer(new List<ServiceDescriptor>())
                        .AddAssembly(typeof(Program).GetTypeInfo().Assembly)
                        .RegisterSundial()
                        .Build();
                var Runner = Canister.Builder.Bootstrapper.Resolve<TimedTaskRunner>();
                Runner.Run();
                Console.ReadKey();
            }
        }
    }
	
The above code creates a TimedTaskRunner object and tells it to run. When that is called it will find all classes that inherit from ITimedTask and profile them. As an example:

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
	
The above bit of code is an example of an ITimedTask class. This one uses Json.NET to deserialize an object. The task has a name and is declared as the Baseline for te series. A series, as denoted by the attribute on the class, is used to group various ITimedTasks together so that they can be compared. It takes a name for the series, the number of iterations to run for, and which exporters to use. In the above code it specifies 1000 iterations and to export to both the console and to a set of html files. After the tasks are run, the results are sent to a set of analyzers and a group of exporters.

## Adding an analyzer

If you want to add a custom analyzer, you simply inherit from the IAnalyzer interface:

    /// <summary>
    /// Max speed analysis
    /// </summary>
    /// <seealso cref="IAnalyzer"/>
    public class MaxSpeedAnalysis : IAnalyzer
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name => "Max speed analysis";

        /// <summary>
        /// Analyzes the specified result.
        /// </summary>
        /// <param name="results">The results.</param>
        /// <returns>The list of findings</returns>
        public IEnumerable<Finding> Analyze(IEnumerable<IResult> results)
        {
            if (results == null || !results.Any())
                return new Finding[0];
            var AverageResult = results.OrderBy(x => x.Percentile("Time", 0.5m).Value).First();
            var Min95Result = results.OrderBy(x => x.Percentile("Time", 0.95m).Value).First();
            if (AverageResult.Name != Min95Result.Name)
                return new Finding[] { new Finding($"\"{AverageResult.Name}\" on average is faster but in the 95% instances, we see \"{Min95Result.Name}\" showing better in the worst case scenarios.") };
            return new Finding[0];
        }
    }
	
The above analyzer is an example from the library itself. It compares the task with the best time at the median and sees if it is also the fastest in the 95th percentile. If it is not, it returns a Finding object letting the user know of the difference. In order to have your analyzer picked up by the system you must add it to Canister:

    Canister.Builder.CreateContainer(new List<ServiceDescriptor>())
                        .AddAssembly(typeof(MaxSpeedAnalysis).GetTypeInfo().Assembly)
                        .RegisterSundial()
                        .Build();
						
With the above, the new analyzer would be picked up by the system.

## Adding an exporter

Adding an exporter is the same as an analyzer but you need to inherit from the IExporter interface:

    /// <summary>
    /// Console exporter
    /// </summary>
    /// <seealso cref="ExporterBaseClass"/>
    public class ConsoleExporter : ExporterBaseClass
    {
        /// <summary>
        /// Gets the file name suffix.
        /// </summary>
        /// <value>The file name suffix.</value>
        public override string FileNameSuffix => "";

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public override string Name => "Console";

        /// <summary>
        /// Runs this instance.
        /// </summary>
        /// <param name="series">The series to export.</param>
        /// <param name="results">The result.</param>
        /// <param name="findings">The findings.</param>
        /// <returns>The various file locations.</returns>
        public override string Export(ISeries series, IEnumerable<IResult> results, IEnumerable<Finding> findings)
        {
            Console.WriteLine("Series: " + series.Name);
            Console.WriteLine();
            Console.WriteLine(results.ToString(x => "* " + x, "\n"));
            Console.WriteLine();
            Console.WriteLine("Analysis:");
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine(findings.ToString(x => "* " + x.Type.ToString() + ": " + x.Description, "\n"));
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine();
            Console.WriteLine();
            return "";
        }

        /// <summary>
        /// Used to write a summary about the various series tested.
        /// </summary>
        /// <param name="summaryData">The summary data.</param>
        /// <returns>The file exported from the system.</returns>
        public override string Summarize(ListMapping<ISeries, IResult> summaryData)
        {
            return "";
        }
    }
	
The one above is the built in Console exporter. Note that it inherits from the ExporterBaseClass which is not necessary. You would register this the same way as the analyzer class above:

    Canister.Builder.CreateContainer(new List<ServiceDescriptor>())
                        .AddAssembly(typeof(ConsoleExporter).GetTypeInfo().Assembly)
                        .RegisterSundial()
                        .Build();

## Installation

The library is available via Nuget with the package name "Sundial.Core". To install it run the following command in the Package Manager Console:

Install-Package Sundial.Core

## Build Process

In order to build the library you will require the following:

1. Visual Studio 2017

Other than that, just clone the project and you should be able to load the solution and build without too much effort.