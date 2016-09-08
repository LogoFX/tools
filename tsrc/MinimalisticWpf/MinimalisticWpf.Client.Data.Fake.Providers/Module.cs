using JetBrains.Annotations;
using MinimalisticWpf.Client.Data.Contracts.Providers;
using Solid.Practices.IoC;
using Solid.Practices.Modularity;

namespace MinimalisticWpf.Client.Data.Fake.Providers
{
    [UsedImplicitly]
    internal sealed class Module : ICompositionModule<IIocContainerRegistrator>
    {
        public void RegisterModule(IIocContainerRegistrator iocContainer)
        {
            iocContainer.RegisterSingleton<IWarehouseProvider, FakeWarehouseProvider>();
            iocContainer.RegisterSingleton<ILoginProvider, FakeLoginProvider>();
        }
    }
}