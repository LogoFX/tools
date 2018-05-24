using Caliburn.Micro;
using LogoFX.Client.Testing.Integration.xUnit;
using $saferootprojectname$.pecifications.Client.Presentation.Shell.ViewModels;

namespace $safeprojectname$
{
    public abstract class IntegrationTestsBaseWithActivation :
        IntegrationTestsBase<ShellViewModel, TestBootstrapper>
    {               
        protected override ShellViewModel CreateRootObjectOverride(ShellViewModel rootObject)
        {
            ScreenExtensions.TryActivate(rootObject);
            return rootObject;
        }        
        
        protected override void OnAfterTeardown()
        {
            base.OnAfterTeardown();
            TestHelper.AfterTeardown();
        }
    }
}