using LogoFX.Client.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.SimpleContainer;
using $saferootprojectname$.pecifications.Client.Launcher.Shared;
using $saferootprojectname$.pecifications.Client.Presentation.Shell.ViewModels;

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
            this.UseResolver().UseShared().Initialize();            
        }

        public override string[] Prefixes => new[] { "Samples.Specifications.Client.Presentation", "$saferootprojectname$.lient.Model", "Samples.Specifications.Client.Data", "Samples.Specifications.Client.Tests", "Samples.Client.Tests", "$saferootprojectname$.pecifications.Tests.Steps" };
    }
}