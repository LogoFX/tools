using LogoFX.Client.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.SimpleContainer;
using LogoFX.Tools.TemplateGenerator.Shell.ViewModels;

namespace LogoFX.Tools.TemplateGenerator.VsExtension
{
    public sealed class AppBootstrapper : BootstrapperContainerBase<SimpleContainerAdapter>
        .WithRootObject<ShellViewModel>
    {
        public AppBootstrapper()
            : base(new SimpleContainerAdapter())
        {
        }
    }
}