using LogoFX.Client.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.SimpleContainer;
using $saferootprojectname$.Client.Data.Contracts.Providers;
using $saferootprojectname$.Client.Model;
using $saferootprojectname$.Client.Model.Contracts;
using $saferootprojectname$.Client.Presentation.Shell.ViewModels;
using Solid.Practices.IoC;

#if FAKE
using $saferootprojectname$.Client.Data.Fake.Providers;
#else
using $saferootprojectname$.Client.Data.Real.Providers;
#endif

namespace $safeprojectname$
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
