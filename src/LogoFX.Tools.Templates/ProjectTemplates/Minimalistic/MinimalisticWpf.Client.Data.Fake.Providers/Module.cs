using JetBrains.Annotations;
using $saferootprojectname$.Client.Data.Contracts.Providers;
using Solid.Practices.IoC;
using Solid.Practices.Modularity;

namespace $safeprojectname$
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