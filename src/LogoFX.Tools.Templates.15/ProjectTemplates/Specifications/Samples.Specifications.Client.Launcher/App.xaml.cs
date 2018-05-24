using LogoFX.Client.Bootstrapping;
using LogoFX.Client.Mvvm.Commanding;
using $safeprojectname$.Shared;

namespace $safeprojectname$
{
    partial class App
    {
        public App()
        {            
            var bootstrapper = new AppBootstrapper();
            bootstrapper
                .UseResolver()
                .UseCommanding()
                .UseShared().Initialize();            
        }
    }
}
