using LogoFX.Client.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.Unity;
using LogoFX.Samples.Specifications.Client.Presentation.Shell.ViewModels;

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
