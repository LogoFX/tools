using LogoFX.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.Unity;
using LogoFX.Client.Mvvm.ViewModel.Services;
using LogoFX.Client.Mvvm.ViewModelFactory.Unity;
using Microsoft.Practices.Unity;
using Solid.Bootstrapping;

namespace $safeprojectname$
{    
    public static class BootstrapperExtensions
    {
        public static IInitializable UseShared(
            this IBootstrapperWithContainer<UnityContainerAdapter, UnityContainer> bootstrapper)             
        {
            return bootstrapper
                .UseViewModelCreatorService()
                .UseViewModelFactory();
        }
    }    
}
