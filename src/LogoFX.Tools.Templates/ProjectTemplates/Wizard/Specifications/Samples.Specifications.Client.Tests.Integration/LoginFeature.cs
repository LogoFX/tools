using $safeprojectname$.Infra;
using $saferootprojectname$.Tests.Steps;
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
        public void NavigateToTheMainScreenWhenTheLoginIsSuccessful()
        {
            var GivenLoginSteps = Resolver.Resolve<GivenLoginSteps>();
            var userName = "Admin";
            var password = "1234";
            GivenLoginSteps.SetupAuthenticatedUserWithCredentials(userName, password);
            GivenLoginSteps.SetupLoginSuccessfullyWithUsername(userName);

            var GeneralSteps = Resolver.Resolve<GeneralSteps>();
            GeneralSteps.WhenIOpenTheApplication();
            var LoginSteps = Resolver.Resolve<LoginSteps>();
            LoginSteps.WhenISetTheUsernameTo(userName);
            LoginSteps.WhenISetThePasswordTo(password);
            LoginSteps.WhenILogInToTheSystem();

            var MainSteps = Resolver.Resolve<MainSteps>();
            MainSteps.ThenApplicationNavigatesToTheMainScreen();
        }
    }
}
