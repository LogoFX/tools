using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using Caliburn.Micro;
using LogoFX.Client.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.SimpleContainer;
using LogoFX.Tools.TemplateGenerator.Shell.ViewModels;
using Solid.Practices.Composition;

namespace LogoFX.Tools.TemplateGenerator.Launcher
{
    public sealed class AppBootstrapper : BootstrapperContainerBase<ExtendedSimpleContainerAdapter>
        .WithRootObject<ShellViewModel>
    {
        public AppBootstrapper()
            : base(new ExtendedSimpleContainerAdapter())
        {
            AssemblyLoadingManager.ClientNamespaces = () => { return new[] {"Shell"}; };
        }
    }
}