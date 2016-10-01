using LogoFX.Client.Testing.Contracts;
using $saferootprojectname$.Client.Data.Fake.ProviderBuilders;
using Solid.Practices.IoC;
using Solid.Practices.Modularity;

namespace $safeprojectname$
{
    class Module : ICompositionModule<IIocContainerRegistrator>
    {
        public void RegisterModule(IIocContainerRegistrator iocContainer)
        {
            iocContainer.RegisterSingleton<IStartApplicationService, StartApplicationService>();
            iocContainer.RegisterInstance(LoginProviderBuilder.CreateBuilder());
            iocContainer.RegisterInstance(WarehouseProviderBuilder.CreateBuilder());
        }
    }
}
