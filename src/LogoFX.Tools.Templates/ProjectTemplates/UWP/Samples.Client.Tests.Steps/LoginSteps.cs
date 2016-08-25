using FluentAssertions;
using $saferootprojectname$.Client.Tests.Contracts.ScreenObjects;

namespace $safeprojectname$
{
    public class LoginSteps
    {
        private readonly ILoginScreenObject _loginScreenObject;

        public LoginSteps(ILoginScreenObject loginScreenObject)
        {
            _loginScreenObject = loginScreenObject;
        }

        public void WhenISetTheUsernameTo(string username)
        {
            _loginScreenObject.SetUsername(username);
        }

        public void WhenILogInToTheSystem()
        {
            _loginScreenObject.Login();
        }

        public void ThenTheLoginScreenIsDisplayed()
        {
            var isActive = _loginScreenObject.IsActive();
            isActive.Should().BeTrue();
        }
    }
}
