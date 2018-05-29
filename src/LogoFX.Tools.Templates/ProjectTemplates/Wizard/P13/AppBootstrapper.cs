using LogoFX.Client.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.SimpleContainer;
using LogoFX.Practices.IoC;
using $saferootprojectname$.Client.Presentation.Shell.ViewModels;

namespace $safeprojectname$
{
    public sealed class AppBootstrapper : BootstrapperContainerBase<ExtendedSimpleContainerAdapter,
        ExtendedSimpleContainer>
        .WithRootObject<ShellViewModel>
    {
        public AppBootstrapper()
            : base(new ExtendedSimpleContainer(), c => new ExtendedSimpleContainerAdapter(c))
        {
        }              
    }
}
