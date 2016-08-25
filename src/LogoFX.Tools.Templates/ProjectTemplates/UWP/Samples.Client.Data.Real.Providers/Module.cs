#if REAL
using JetBrains.Annotations;
using $saferootprojectname$.Client.Data.Contracts.Providers;
using Solid.Practices.IoC;
using Solid.Practices.Modularity;

namespace $safeprojectname$
{
    [UsedImplicitly]
    class Module : ICompositionModule<IIocContainerRegistrator>
    {
        public void RegisterModule(IIocContainerRegistrator iocContainer)
        {
            iocContainer.RegisterSingleton<ILoginProvider, LoginProvider>();
            iocContainer.RegisterSingleton<IWarehouseProvider, WarehouseProvider>();            
        }
    }
}
#endif