using FileCurator.Registration;
using Microsoft.Extensions.DependencyInjection;
using SerialBox.Registration;
using Sundial.Core.Registration;
using Sundial.Core.Runner;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace TestApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Canister.Builder.CreateContainer(new List<ServiceDescriptor>())
                    .AddAssembly(typeof(Program).GetTypeInfo().Assembly)
                    .RegisterSundial()
                    .RegisterFileCurator()
                    .RegisterSerialBox()
                    .Build();
            var Runner = Canister.Builder.Bootstrapper.Resolve<TimedTaskRunner>();
            Runner.Run();
            Console.ReadKey();
        }
    }
}