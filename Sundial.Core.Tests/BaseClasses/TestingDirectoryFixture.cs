using BigBook.ExtensionMethods;
using FileCurator;
using FileCurator.Registration;
using Microsoft.Extensions.DependencyInjection;
using Sundial.Core.Registration;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace Sundial.Core.Tests.BaseClasses
{
    [Collection("BasicCollection")]
    public class TestingDirectoryFixture : IDisposable
    {
        public TestingDirectoryFixture()
        {
            if (Canister.Builder.Bootstrapper == null)
                Canister.Builder.CreateContainer(new List<ServiceDescriptor>())
                    .AddAssembly(typeof(TestingDirectoryFixture).GetTypeInfo().Assembly)
                    .RegisterSundial()
                    .RegisterFileCurator()
                    .Build();
            new DirectoryInfo(@".\Testing").Create();
            ((object)null).Cache("Root_Profiler", "Item");
            ((object)null).Cache("Current_Profiler", "Item");
        }

        public void Dispose()
        {
            ((object)null).Cache("Root_Profiler", "Item");
            ((object)null).Cache("Current_Profiler", "Item");
            new DirectoryInfo(@".\Testing").Delete();
        }
    }
}