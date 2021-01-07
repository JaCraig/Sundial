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
            new ServiceCollection().AddLogging().AddCanisterModules(x => x.AddAssembly(typeof(Program).GetTypeInfo().Assembly)
                    .RegisterSundial()
                    .RegisterFileCurator()
                    .RegisterSerialBox());
            var Runner = Canister.Builder.Bootstrapper.Resolve<TimedTaskRunner>();
            Runner.Run();
            Console.ReadKey();
        }
    }
}