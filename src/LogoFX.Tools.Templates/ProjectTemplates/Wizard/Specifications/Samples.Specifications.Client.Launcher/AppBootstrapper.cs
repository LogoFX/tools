#define Unity

using LogoFX.Client.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.Unity;
using $saferootprojectname$.Client.Presentation.Shell.ViewModels;

namespace $safeprojectname$
{
    public class AppBootstrapper : BootstrapperContainerBase<UnityContainerAdapter>.WithRootObject<ShellViewModel>
    {
        public AppBootstrapper(UnityContainerAdapter containerAdapter) 
            : base(containerAdapter)
        {
        }              
    }
}
