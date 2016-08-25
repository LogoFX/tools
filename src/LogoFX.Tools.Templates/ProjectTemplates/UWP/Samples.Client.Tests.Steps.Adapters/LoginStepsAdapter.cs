using TechTalk.SpecFlow;

namespace $safeprojectname$
{
    [Binding]
    public sealed class LoginStepsAdapter
    {
        public LoginSteps LoginSteps { get; set; }

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

        [Then(@"the login screen is displayed")]
        public void ThenTheLoginScreenIsDisplayed()
        {
            LoginSteps.ThenTheLoginScreenIsDisplayed();
        }        
    }
}
