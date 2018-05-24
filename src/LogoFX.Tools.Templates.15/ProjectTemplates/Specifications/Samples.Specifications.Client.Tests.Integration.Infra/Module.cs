using LogoFX.Client.Testing.Contracts;
using LogoFX.Client.Testing.Integration.xUnit;
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
                .AddSingleton<IBuilderRegistrationService, BuilderRegistrationService>()
                .AddSingleton<IStartApplicationService, StartApplicationService>()
                .RegisterBuilders();
        }
    }
}
