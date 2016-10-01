using LogoFX.Client.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.Unity;
using $safeprojectname$.Shared;

namespace $safeprojectname$
{
    partial class App
    {
        public App()
        {            
            var bootstrapper = new AppBootstrapper(new UnityContainerAdapter());
            bootstrapper.UseResolver().UseShared().Initialize();            
        }
    }
}
