using LogoFX.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.Unity;
using LogoFX.Client.Mvvm.ViewModel.Services;
using LogoFX.Client.Mvvm.ViewModelFactory.Unity;

namespace $safeprojectname$
{    
    public static class BootstrapperExtensions
    {
        public static IBootstrapperWithContainerAdapter<UnityContainerAdapter> UseShared(
            this IBootstrapperWithContainerAdapter<UnityContainerAdapter> bootstrapper)
        {
            return bootstrapper.UseViewModelCreatorService().UseViewModelFactory();
        }
    }
}
