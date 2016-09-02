using LogoFX.Client.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.SimpleContainer;
using LogoFX.Tools.TemplateGenerator.Contracts;
using LogoFX.Tools.TemplateGenerator.Shell.ViewModels;
using Solid.Practices.IoC;

namespace LogoFX.Tools.TemplateGenerator.Launcher
{
    public sealed class AppBootstrapper : BootstrapperContainerBase<SimpleContainerAdapter>
        .WithRootObject<ShellViewModel>
    {
        public AppBootstrapper()
            : base(new SimpleContainerAdapter())
        {
        }

        protected override void OnConfigure(IIocContainerRegistrator containerRegistrator)
        {
            base.OnConfigure(containerRegistrator);
            containerRegistrator.RegisterSingleton<IDataService, DataService>();
        }
    }
}