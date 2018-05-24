using LogoFX.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.SimpleContainer;
using LogoFX.Client.Mvvm.ViewModel.Services;
using LogoFX.Client.Mvvm.ViewModelFactory.SimpleContainer;
using Solid.Bootstrapping;

namespace $safeprojectname$
{    
    public static class BootstrapperExtensions
    {
        public static IInitializable UseShared(
            this IBootstrapperWithContainerAdapter<ExtendedSimpleContainerAdapter> bootstrapper)             
        {
            return bootstrapper
                .UseViewModelCreatorService()
                .UseViewModelFactory();
        }
    }    
}
