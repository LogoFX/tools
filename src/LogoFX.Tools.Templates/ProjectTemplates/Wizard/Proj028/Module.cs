using Solid.Practices.IoC;
using Solid.Practices.Modularity;

namespace $safeprojectname$
{
    class Module : ICompositionModule<IIocContainerRegistrator>
    {
        public void RegisterModule(IIocContainerRegistrator iocContainer)
        {
            iocContainer.RegisterSingleton<GeneralSteps, GeneralSteps>();
            iocContainer.RegisterSingleton<GivenLoginSteps, GivenLoginSteps>();
            iocContainer.RegisterSingleton<LoginSteps, LoginSteps>();            
            iocContainer.RegisterSingleton<MainSteps, MainSteps>();
            iocContainer.RegisterSingleton<ExitSteps, ExitSteps>();
        }
    }
}
