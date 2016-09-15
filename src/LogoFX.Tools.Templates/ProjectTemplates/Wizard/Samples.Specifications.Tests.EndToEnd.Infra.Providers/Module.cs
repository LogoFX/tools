using JetBrains.Annotations;
using LogoFX.Client.Testing.EndToEnd.FakeData.Modularity;
using $saferootprojectname$.Client.Data.Fake.ProviderBuilders;
using Solid.Practices.IoC;

namespace $safeprojectname$
{    
    [UsedImplicitly]
    public class Module : ProvidersModuleBase
    {
        protected override void OnRegisterProviders(IIocContainerRegistrator iocContainer)
        {
            base.OnRegisterProviders(iocContainer);
            RegisterAllBuilders(iocContainer, LoginProviderBuilder.CreateBuilder);
            RegisterAllBuilders(iocContainer, WarehouseProviderBuilder.CreateBuilder);            
        }
    }
}
