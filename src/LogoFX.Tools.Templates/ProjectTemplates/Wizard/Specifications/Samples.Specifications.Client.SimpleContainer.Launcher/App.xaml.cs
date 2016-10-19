using LogoFX.Client.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.SimpleContainer;
using $safeprojectname$.Shared;

namespace $safeprojectname$
{
    partial class App
    {
        public App()
        {            
            var bootstrapper = new AppBootstrapper(new SimpleContainerAdapter());
            bootstrapper.UseResolver().UseShared().Initialize();            
        }
    }
}
