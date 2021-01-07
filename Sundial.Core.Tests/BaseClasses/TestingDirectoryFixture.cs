using BigBook.ExtensionMethods;
using FileCurator;
using Microsoft.Extensions.DependencyInjection;
using System;
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
            {
                new ServiceCollection().AddLogging()
                    .AddCanisterModules(x => x.AddAssembly(typeof(TestingDirectoryFixture).GetTypeInfo().Assembly)
                                              .RegisterSundial()
                                              .RegisterFileCurator());
            }

            new DirectoryInfo(@".\Testing").Create();
            ((object)null).Cache("Root_Profiler", "Item");
            ((object)null).Cache("Current_Profiler", "Item");
        }

        protected static BigBook.Caching.Manager CacheManager => Canister.Builder.Bootstrapper.Resolve<BigBook.Caching.Manager>();

        public void Dispose()
        {
            ((object)null).Cache("Root_Profiler", "Item");
            ((object)null).Cache("Current_Profiler", "Item");
            new DirectoryInfo(@".\Testing").Delete();
        }
    }
}