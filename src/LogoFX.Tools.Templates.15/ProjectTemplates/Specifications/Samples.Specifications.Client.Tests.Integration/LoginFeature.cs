using $safeprojectname$.Infra;
using $saferootprojectname$.pecifications.Tests.Steps;
using Xunit;

namespace $safeprojectname$
{
    public class LoginFeature : IntegrationTestsBaseWithActivation
    {
        [Fact]
        public void LoginScreenIsDisplayedFirst()
        {
            var generalSteps = Resolver.Resolve<GeneralSteps>();
            generalSteps.WhenIOpenTheApplication();

            var loginSteps = Resolver.Resolve<LoginSteps>();
            loginSteps.ThenTheLoginScreenIsDisplayed();
        }

        [Fact]
        public void NavigateToTheMainScreenWhenTheLoginIsSuccessful()
        {
            var givenLoginSteps = Resolver.Resolve<GivenLoginSteps>();
            var userName = "Admin";
            var password = "pass";
            givenLoginSteps.SetupAuthenticatedUserWithCredentials(userName, password);            

            var generalSteps = Resolver.Resolve<GeneralSteps>();
            generalSteps.WhenIOpenTheApplication();
            var loginSteps = Resolver.Resolve<LoginSteps>();
            loginSteps.WhenISetTheUsernameTo(userName);
            loginSteps.WhenISetThePasswordTo(password);
            loginSteps.WhenILogInToTheSystem();

            var mainSteps = Resolver.Resolve<MainSteps>();
            mainSteps.ThenApplicationNavigatesToTheMainScreen();
        }
    }
}
