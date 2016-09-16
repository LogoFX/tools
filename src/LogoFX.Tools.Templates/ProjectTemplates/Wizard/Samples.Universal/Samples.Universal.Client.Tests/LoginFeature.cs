using $saferootprojectname$.Client.Tests.Steps;
using $safeprojectname$.Integration.Infra;
using Xunit;

namespace $safeprojectname$
{    
    public class LoginFeature : IntegrationTestsBaseWithActivation
    {
        [Fact]
        public void LoginScreenIsDisplayedFirst()
        {
            var GeneralSteps = Resolver.Resolve<GeneralSteps>();
            GeneralSteps.WhenIOpenTheApplication();

            var LoginSteps = Resolver.Resolve<LoginSteps>();
            LoginSteps.ThenTheLoginScreenIsDisplayed();
        }

        [Fact]
        public void NavigateToTheMainScreenWheTheLoginIsSuccessful()
        {
            var GivenLoginSteps = Resolver.Resolve<GivenLoginSteps>();
            var userName = "Admin";            
            GivenLoginSteps.SetupAuthenticatedUserWithUsername(userName);
            GivenLoginSteps.SetupLoginSuccessfullyWithUsername(userName);

            var GeneralSteps = Resolver.Resolve<GeneralSteps>();
            GeneralSteps.WhenIOpenTheApplication();
            var LoginSteps = Resolver.Resolve<LoginSteps>();
            LoginSteps.WhenISetTheUsernameTo(userName);
            LoginSteps.WhenILogInToTheSystem();

            var MainSteps = Resolver.Resolve<MainSteps>();
            MainSteps.ThenApplicationNavigatesToTheMainScreen();
        }
    }
}
