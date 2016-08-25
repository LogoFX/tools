using LogoFX.Client.Testing.Contracts;
using LogoFX.Client.Testing.Integration.xUnit;
using Solid.Practices.IoC;
using Solid.Practices.Modularity;

namespace $safeprojectname$
{
    class Module : ICompositionModule<IIocContainerRegistrator>
    {
        public void RegisterModule(IIocContainerRegistrator iocContainer)
        {
            iocContainer.RegisterSingleton<IBuilderRegistrationService, BuilderRegistrationService>();
        }
    }
}
