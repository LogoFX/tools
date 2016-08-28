using LogoFX.Client.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.SimpleContainer;
using LogoFX.Tools.TemplateGenerator.Shell.ViewModels;

namespace LogoFX.Tools.TemplateGenerator.VsExtension.Bridge
{
    internal sealed class AppBootstrapperInternal : BootstrapperContainerBase<SimpleContainerAdapter>
        .WithRootObject<ShellViewModel>
    {
        public AppBootstrapperInternal()
            : base(new SimpleContainerAdapter())
        {
        }
    }

    public sealed class AppBootstrapper
    {
        private readonly AppBootstrapperInternal _appBootstrapperInternal;

        public AppBootstrapper()
        {
            _appBootstrapperInternal = new AppBootstrapperInternal();
            _appBootstrapperInternal.UseResolver().Initialize();

        }
    }
}