using LogoFX.Client.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.SimpleContainer;
using LogoFX.Client.Mvvm.ViewModel.Services;
using LogoFX.Client.Mvvm.ViewModelFactory.SimpleContainer;
using $saferootprojectname$.Client.Presentation.Shell.ViewModels;

namespace $safeprojectname$
{
    public class TestBootstrapper : TestBootstrapperContainerBase<ExtendedSimpleContainerAdapter>
        .WithRootObject<ShellViewModel>
    {
        public TestBootstrapper() :
            base(new ExtendedSimpleContainerAdapter(), new BootstrapperCreationOptions
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