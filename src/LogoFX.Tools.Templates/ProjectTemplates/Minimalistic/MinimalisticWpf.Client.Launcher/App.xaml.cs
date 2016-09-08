using LogoFX.Client.Bootstrapping;
using LogoFX.Client.Mvvm.ViewModel.Services;
using LogoFX.Client.Mvvm.ViewModelFactory.SimpleContainer;

namespace $safeprojectname$
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
            var bootstrapper = new AppBootstrapper();
            bootstrapper
                .UseResolver()
                .UseViewModelCreatorService()
                .UseViewModelFactory()
                .Initialize();
        }
    }
}
