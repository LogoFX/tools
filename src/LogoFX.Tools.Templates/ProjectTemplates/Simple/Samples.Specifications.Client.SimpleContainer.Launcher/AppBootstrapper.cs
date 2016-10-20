using LogoFX.Client.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.SimpleContainer;
using $saferootprojectname$.Client.Presentation.Shell.ViewModels;

namespace $safeprojectname$
{
    public class AppBootstrapper : BootstrapperContainerBase<SimpleContainerAdapter>.WithRootObject<ShellViewModel>
    {
        public AppBootstrapper(SimpleContainerAdapter containerAdapter) 
            : base(containerAdapter)
        {
        }              
    }
}
