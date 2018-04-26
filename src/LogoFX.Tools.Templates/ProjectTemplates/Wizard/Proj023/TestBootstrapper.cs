using LogoFX.Client.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.SimpleContainer;
using LogoFX.Practices.IoC;
using $saferootprojectname$.Client.Launcher.Shared;
using $saferootprojectname$.Client.Presentation.Shell.ViewModels;

namespace $safeprojectname$
{
   public class TestBootstrapper : TestBootstrapperContainerBase<ExtendedSimpleContainerAdapter,
        ExtendedSimpleContainer>
        .WithRootObject<ShellViewModel>
    {
        public TestBootstrapper() :
            base(new ExtendedSimpleContainer(), c => new ExtendedSimpleContainerAdapter(c), new BootstrapperCreationOptions
            {
                UseApplication = false,
                ReuseCompositionInformation = true
            })
        {
            this.UseResolver();
            this.UseShared();
            this.Initialize();
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