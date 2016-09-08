using JetBrains.Annotations;
using $safeprojectname$.Contracts;
using Solid.Practices.IoC;
using Solid.Practices.Modularity;

namespace $safeprojectname$
{
    [UsedImplicitly]
    internal sealed class Module : ICompositionModule<IIocContainerRegistrator>
    {
        public void RegisterModule(IIocContainerRegistrator iocContainer)
        {
            iocContainer.RegisterSingleton<IDataService, DataService>();
            iocContainer.RegisterSingleton<ILoginService, LoginService>();
        }
    }
}