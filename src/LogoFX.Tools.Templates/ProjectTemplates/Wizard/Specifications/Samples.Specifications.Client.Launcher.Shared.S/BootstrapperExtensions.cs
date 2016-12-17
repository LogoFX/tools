using LogoFX.Bootstrapping;
using LogoFX.Client.Mvvm.ViewModel.Services;
using LogoFX.Client.Mvvm.ViewModelFactory.SimpleContainer;
using Solid.Practices.IoC;

namespace $safeprojectname$
{    
    public static class BootstrapperExtensions
    {
        public static IBootstrapperWithContainerAdapter<TContainerAdapter> UseShared<TContainerAdapter>(
            this IBootstrapperWithContainerAdapter<TContainerAdapter> bootstrapper) where TContainerAdapter : IIocContainer
        {
            return bootstrapper.UseViewModelCreatorService().UseViewModelFactory();
        }
    }
}
