using Caliburn.Micro;
using LogoFX.Client.Bootstrapping;
using LogoFX.Client.Mvvm.ViewModel.Services;

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
                .UseViewModelCreatorService()
                .Initialize();
        }
    }
}