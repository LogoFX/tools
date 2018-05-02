using LogoFX.Client.Bootstrapping;
using LogoFX.Client.Mvvm.Commanding;
using LogoFX.Client.Mvvm.ViewModel.Services;
using LogoFX.Client.Mvvm.ViewModelFactory.SimpleContainer;

namespace LogoFX.Tools.TemplateGenerator.Launcher
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
                .UseCommanding()
                .UseViewModelCreatorService()
                .UseViewModelFactory()
                .Initialize();
        }
    }
}