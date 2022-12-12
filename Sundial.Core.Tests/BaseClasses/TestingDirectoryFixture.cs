using DragonHoard.Core;
using FileCurator;
using Microsoft.Extensions.DependencyInjection;
using Sundial.Core.Manager.Default;
using System;
using Xunit;

namespace Sundial.Core.Tests.BaseClasses
{
    [Collection("BasicCollection")]
    public class TestingDirectoryFixture : IDisposable
    {
        public TestingDirectoryFixture()
        {
            new DirectoryInfo(@".\Testing").Create();
            CacheManager.GetOrAddCache("Item")?.Set<InternalProfiler>("Root_Profiler", null);
            CacheManager.GetOrAddCache("Item")?.Set<InternalProfiler>("Current_Profiler", null);
        }

        protected static Cache CacheManager => GetServiceProvider().GetService<Cache>();

        /// <summary>
        /// The service provider lock
        /// </summary>
        private static readonly object ServiceProviderLock = new object();

        /// <summary>
        /// The service provider
        /// </summary>
        private static IServiceProvider ServiceProvider;

        public void Dispose()
        {
            CacheManager.GetOrAddCache("Item")?.Set<InternalProfiler>("Root_Profiler", null);
            CacheManager.GetOrAddCache("Item")?.Set<InternalProfiler>("Current_Profiler", null);
            new DirectoryInfo(@".\Testing").Delete();
        }

        /// <summary>
        /// Gets the service provider.
        /// </summary>
        /// <returns></returns>
        protected static IServiceProvider GetServiceProvider()
        {
            if (ServiceProvider is not null)
                return ServiceProvider;
            lock (ServiceProviderLock)
            {
                if (ServiceProvider is not null)
                    return ServiceProvider;
                ServiceProvider = new ServiceCollection().AddCanisterModules()?.BuildServiceProvider();
            }
            return ServiceProvider;
        }
    }
}