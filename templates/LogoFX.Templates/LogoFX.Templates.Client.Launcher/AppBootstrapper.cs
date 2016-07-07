using LogoFX.Client.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.Unity;
using LogoFX.Templates.Client.Shell.ViewModels;

namespace LogoFX.Templates.Client.Launcher
{
    public class AppBootstrapper : BootstrapperContainerBase<UnityContainerAdapter>.WithRootObject<ShellViewModel>
    {
        public AppBootstrapper(UnityContainerAdapter containerAdapter)
            : base(containerAdapter)
        {
        }
    }
}