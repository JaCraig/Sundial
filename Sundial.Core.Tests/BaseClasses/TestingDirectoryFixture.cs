using DragonHoard.Core;
using FileCurator;
using Microsoft.Extensions.DependencyInjection;
using Sundial.Core.Manager.Default;
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
                                                .RegisterInMemoryHoard()
                                              .RegisterSundial()
                                              .RegisterFileCurator());
            }

            new DirectoryInfo(@".\Testing").Create();
            CacheManager.GetOrAddCache("Item")?.Set<InternalProfiler>("Root_Profiler", null);
            CacheManager.GetOrAddCache("Item")?.Set<InternalProfiler>("Current_Profiler", null);
        }

        protected static Cache CacheManager => Canister.Builder.Bootstrapper.Resolve<Cache>();

        public void Dispose()
        {
            CacheManager.GetOrAddCache("Item")?.Set<InternalProfiler>("Root_Profiler", null);
            CacheManager.GetOrAddCache("Item")?.Set<InternalProfiler>("Current_Profiler", null);
            new DirectoryInfo(@".\Testing").Delete();
        }
    }
}