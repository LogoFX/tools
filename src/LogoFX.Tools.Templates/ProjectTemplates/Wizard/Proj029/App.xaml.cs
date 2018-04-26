using LogoFX.Client.Bootstrapping;
using $safeprojectname$.Shared;

namespace $safeprojectname$
{
    partial class App
    {
        public App()
        {            
            var bootstrapper = new AppBootstrapper();
            bootstrapper.UseResolver();
            bootstrapper.UseShared().Initialize();
        }
    }
}
