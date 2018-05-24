using LogoFX.Client.Testing.Contracts;
using LogoFX.Client.Testing.EndToEnd;
using LogoFX.Client.Testing.EndToEnd.FakeData;
using $saferootprojectname$.pecifications.Client.Data.Fake.Shared;
using Solid.Practices.IoC;
using Solid.Practices.Modularity;

namespace $safeprojectname$
{
    class Module : ICompositionModule<IDependencyRegistrator>
    {                
        public void RegisterModule(IDependencyRegistrator dependencyRegistrator)
        {
            dependencyRegistrator
                .AddSingleton<IStartApplicationService, StartApplicationService.WithFakeProviders>()
                .AddSingleton<IBuilderRegistrationService, BuilderRegistrationService>()
                .RegisterBuilders();                        
        }        
    }
}
