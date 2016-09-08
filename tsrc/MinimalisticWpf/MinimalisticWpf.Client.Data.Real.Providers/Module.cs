using JetBrains.Annotations;
using MinimalisticWpf.Client.Data.Contracts.Providers;
using Solid.Practices.IoC;
using Solid.Practices.Modularity;

namespace MinimalisticWpf.Client.Data.Real.Providers
{
    [UsedImplicitly]
    internal sealed class Module : ICompositionModule<IIocContainerRegistrator>
    {
        public void RegisterModule(IIocContainerRegistrator iocContainer)
        {
            iocContainer.RegisterSingleton<IWarehouseProvider, WarehouseProvider>();
            iocContainer.RegisterSingleton<ILoginProvider, LoginProvider>();
        }
    }
}