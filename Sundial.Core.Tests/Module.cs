using Canister.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Sundial.Core.Tests
{
    public class Module : IModule
    {
        public int Order { get; }

        public void Load(IServiceCollection serviceDescriptors)
        {
            serviceDescriptors?.AddLogging();
        }
    }
}