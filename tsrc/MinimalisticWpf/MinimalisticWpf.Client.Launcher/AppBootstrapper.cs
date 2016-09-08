using LogoFX.Client.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.SimpleContainer;
using MinimalisticWpf.Client.Data.Contracts.Providers;
using MinimalisticWpf.Client.Model;
using MinimalisticWpf.Client.Model.Contracts;
using MinimalisticWpf.Client.Presentation.Shell.ViewModels;
using Solid.Practices.IoC;

#if FAKE
using MinimalisticWpf.Client.Data.Fake.Providers;
#else
using MinimalisticWpf.Client.Data.Real.Providers;
#endif

namespace MinimalisticWpf.Client.Launcher
{
    public class AppBootstrapper : BootstrapperContainerBase<ExtendedSimpleContainerAdapter>
        .WithRootObject<ShellViewModel>
    {
        public AppBootstrapper()
            : base(new ExtendedSimpleContainerAdapter())
        {
        }

        protected override void OnConfigure(IIocContainerRegistrator containerRegistrator)
        {
            base.OnConfigure(containerRegistrator);

#if FAKE
            containerRegistrator.RegisterSingleton<ILoginProvider, FakeLoginProvider>();
            containerRegistrator.RegisterSingleton<IWarehouseProvider, FakeWarehouseProvider>();
#else
            containerRegistrator.RegisterSingleton<ILoginProvider, LoginProvider>();
            containerRegistrator.RegisterSingleton<IWarehouseProvider, WarehouseProvider>();
#endif
            containerRegistrator.RegisterSingleton<IDataService, DataService>();
            containerRegistrator.RegisterSingleton<ILoginService, LoginService>();
        }
    }
}
