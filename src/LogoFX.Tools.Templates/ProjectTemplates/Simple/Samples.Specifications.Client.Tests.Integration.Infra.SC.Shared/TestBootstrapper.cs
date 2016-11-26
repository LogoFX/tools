using LogoFX.Client.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.SimpleContainer;
using Samples.Specifications.Client.Launcher.Shared;
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
            this.UseResolver().UseShared();                               
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