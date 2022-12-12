using Microsoft.Extensions.DependencyInjection;
using Sundial.Core.Runner;
using System;
using System.Reflection;

namespace TestApp
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var Services = new ServiceCollection().AddLogging().AddCanisterModules(x => x.AddAssembly(typeof(Program).GetTypeInfo().Assembly)
                    .RegisterSundial()
                    .RegisterFileCurator()
                    .RegisterSerialBox()).BuildServiceProvider();
            var Runner = Services.GetService<TimedTaskRunner>();
            Runner.Run();
            Console.ReadKey();
        }
    }
}