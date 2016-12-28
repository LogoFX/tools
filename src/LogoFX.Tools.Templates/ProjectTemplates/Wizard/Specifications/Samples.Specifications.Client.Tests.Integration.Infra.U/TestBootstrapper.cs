using LogoFX.Client.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.Unity;
using $saferootprojectname$.Client.Presentation.Shell.ViewModels;
using LogoFX.Client.Mvvm.ViewModel.Services;
using LogoFX.Client.Mvvm.ViewModelFactory.Unity;

namespace $safeprojectname$
{
    public class TestBootstrapper : TestBootstrapperContainerBase<UnityContainerAdapter>
        .WithRootObject<ShellViewModel>
    {
        public TestBootstrapper() :
            base(new UnityContainerAdapter(), new BootstrapperCreationOptions
            {
                UseApplication = false,
                ReuseCompositionInformation = true
            })
        {			
            this.UseResolver().UseViewModelCreatorService().UseViewModelFactory();
        }

        public override string[] Prefixes
        {
            get
            {
                return new[] { "$saferootprojectname$.Client.Presentation", "$saferootprojectname$.Client.Model", "$saferootprojectname$.Client.Data", "$saferootprojectname$.Client.Tests", "$saferootprojectname$.Client.Tests", "$saferootprojectname$.Tests.Steps" };
            }
        }
    }
}