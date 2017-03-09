using LogoFX.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.SimpleContainer;
using LogoFX.Client.Mvvm.ViewModel.Services;
using LogoFX.Client.Mvvm.ViewModelFactory.SimpleContainer;
using LogoFX.Practices.IoC;
using Solid.Bootstrapping;

namespace $safeprojectname$
{    
    public static class BootstrapperExtensions
    {
        public static IInitializable UseShared(
            this IBootstrapperWithContainer<ExtendedSimpleContainerAdapter, ExtendedSimpleContainer> bootstrapper)             
        {
            return bootstrapper
                .UseViewModelCreatorService()
                .UseViewModelFactory();
        }
    }    
}
