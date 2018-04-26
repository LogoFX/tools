using LogoFX.Client.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.Unity;
using LogoFX.Practices.IoC;
using Microsoft.Practices.Unity;
using $saferootprojectname$.Client.Presentation.Shell.ViewModels;

namespace $safeprojectname$
{
    public sealed class AppBootstrapper : BootstrapperContainerBase<UnityContainerAdapter, UnityContainer>
        .WithRootObject<ShellViewModel>
    {
        public AppBootstrapper()
            : base(new UnityContainer(), c => new UnityContainerAdapter())
        {
        }
    }
}
