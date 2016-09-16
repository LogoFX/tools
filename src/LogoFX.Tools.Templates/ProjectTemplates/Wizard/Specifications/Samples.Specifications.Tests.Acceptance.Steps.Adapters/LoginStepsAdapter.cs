using $saferootprojectname$.Tests.Steps;
using TechTalk.SpecFlow;

namespace $safeprojectname$
{
    [Binding]
    public sealed class LoginStepsAdapter
    {
        private LoginSteps LoginSteps { get; set; }

        public LoginStepsAdapter(LoginSteps loginSteps)
        {
            LoginSteps = loginSteps;
        }

        [When(@"I set the username to ""(.*)""")]
        public void WhenISetTheUsernameTo(string username)
        {
            LoginSteps.WhenISetTheUsernameTo(username);
        }

        [When(@"I log in to the system")]
        public void WhenILogInToTheSystem()
        {
            LoginSteps.WhenILogInToTheSystem();
        }

        [When(@"I set the password to ""(.*)""")]
        public void WhenISetThePasswordTo(string password)
        {
            LoginSteps.WhenISetThePasswordTo(password);
        }

        [Then(@"the login screen is displayed")]
        public void ThenTheLoginScreenIsDisplayed()
        {
            LoginSteps.ThenTheLoginScreenIsDisplayed();            
        }

        [Then(@"Application navigates to the main screen")]
        public void ThenApplicationNavigatesToTheMainScreen()
        {
            //for readability reasons
        }
    }
}
