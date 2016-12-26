using LogoFX.Client.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.Unity;
using LogoFX.Client.Mvvm.ViewModel.Services;
using LogoFX.Client.Mvvm.ViewModelFactory.Unity;
using $saferootprojectname$.Client.Presentation.Shell.ViewModels;

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
                return new[] { "Samples.Specifications.Client.Presentation", "$saferootprojectname$.Client.Model", "Samples.Specifications.Client.Data", "Samples.Specifications.Client.Tests", "Samples.Client.Tests" };
            }
        }
    }
}