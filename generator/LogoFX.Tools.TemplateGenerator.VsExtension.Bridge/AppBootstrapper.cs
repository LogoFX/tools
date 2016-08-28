using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using LogoFX.Tools.TemplateGenerator.Shell.ViewModels;

namespace LogoFX.Tools.TemplateGenerator.VsExtension.Bridge
{
    public sealed class AppBootstrapper : BootstrapperBase
    {
        public AppBootstrapper()
        {
            Initialize();

            DelayedStartup();
        }

        private async void DelayedStartup()
        {
            await Task.Delay(500);
            OnStartup(Application, null);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }
    }
}