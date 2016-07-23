using $saferootprojectname$.Tests.Acceptance.Contracts.ScreenObjects;
using $safeprojectname$.ScreenObjects;
using Solid.Practices.IoC;
using Solid.Practices.Modularity;

namespace $safeprojectname$
{
    class Module : ICompositionModule<IIocContainerRegistrator>
    {
        public void RegisterModule(IIocContainerRegistrator iocContainer)
        {
            iocContainer.RegisterSingleton<ILoginScreenObject, LoginScreenObject>();
            iocContainer.RegisterSingleton<IShellScreenObject, ShellScreenObject>();
            iocContainer.RegisterSingleton<IMainScreenObject, MainScreenObject>();
        }
    }
}
