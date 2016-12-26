using LogoFX.Client.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.Unity;
using LogoFX.Client.Mvvm.ViewModel.Services;
using LogoFX.Client.Mvvm.ViewModelFactory.Unity;

namespace $safeprojectname$
{
    partial class App
    {
        public App()
        {            
            var bootstrapper = new AppBootstrapper(new UnityContainerAdapter());
            bootstrapper
                .UseResolver()
                .UseViewModelCreatorService()
                .UseViewModelFactory()
                .Initialize();            
        }
    }
}
