using LogoFX.Client.Testing.Contracts;
using LogoFX.Client.Testing.Integration.xUnit;
using $saferootprojectname$.Client.Data.Fake.ProviderBuilders;
using Solid.Practices.IoC;
using Solid.Practices.Modularity;

namespace $safeprojectname$
{
    class Module : ICompositionModule<IIocContainerRegistrator>
    {
        public void RegisterModule(IIocContainerRegistrator iocContainer)
        {
            iocContainer.RegisterSingleton<IBuilderRegistrationService, BuilderRegistrationService>();
            iocContainer.RegisterSingleton<IStartApplicationService, StartApplicationService>();
            iocContainer.RegisterInstance(LoginProviderBuilder.CreateBuilder());
            iocContainer.RegisterInstance(WarehouseProviderBuilder.CreateBuilder());
        }
    }
}
