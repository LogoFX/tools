using LogoFX.Client.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.SimpleContainer;
using $safeprojectname$.Shared;

namespace $safeprojectname$
{
    partial class App
    {
        public App()
        {            
            var bootstrapper = new AppBootstrapper(new ExtendedSimpleContainerAdapter());
            bootstrapper.UseResolver().UseShared().Initialize();            
        }
    }
}
