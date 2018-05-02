using System.Threading;
using System.Windows;
using LogoFX.Client.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.SimpleContainer;
using LogoFX.Tools.TemplateGenerator.Shell.ViewModels;
using Solid.Practices.Composition;
using Solid.Practices.IoC;

namespace LogoFX.Tools.TemplateGenerator.Launcher
{
    public sealed class AppBootstrapper : BootstrapperContainerBase<ExtendedSimpleContainerAdapter>
        .WithRootObject<ShellViewModel>
    {
        public AppBootstrapper()
            : base(new ExtendedSimpleContainerAdapter())
        {
            AssemblyLoadingManager.ClientNamespaces = () => new[] {"Shell"};
        }

        public override string[] Prefixes
        {
            get { return new[] {"LogoFX.Tools.TemplateGenerator"}; }
        }
    }
}