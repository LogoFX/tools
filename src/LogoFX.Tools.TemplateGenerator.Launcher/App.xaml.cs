using LogoFX.Client.Bootstrapping;

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
            bootstrapper.UseResolver().Initialize();
        }
    }
}