using JetBrains.Annotations;
using $safeprojectname$.Contracts;
using Solid.Practices.IoC;
using Solid.Practices.Modularity;

namespace $safeprojectname$
{
    [UsedImplicitly]
    class Module : ICompositionModule<IIocContainerRegistrator>
    {
        public void RegisterModule(IIocContainerRegistrator iocContainer)
        {                        
            iocContainer.RegisterSingleton<ILoginService, LoginService>();
            iocContainer.RegisterSingleton<IDataService, DataService>();
        }
    }
}
