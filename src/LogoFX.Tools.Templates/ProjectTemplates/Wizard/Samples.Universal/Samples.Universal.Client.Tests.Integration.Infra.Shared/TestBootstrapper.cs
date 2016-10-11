using LogoFX.Client.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.SimpleContainer;
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
            
        }

        public override string[] Prefixes
        {
            get
            {
                return new[] { "Samples.Universal.Client.Presentation", "$saferootprojectname$.Client.Model", "Samples.Client.Data", "$saferootprojectname$.Client.Tests", "Samples.Client.Tests" };
            }
        }
    }
}