using FluentAssertions;
using $saferootprojectname$.Client.Tests.Contracts.ScreenObjects;

namespace $safeprojectname$
{
    public class MainSteps
    {        
        private readonly IMainScreenObject _mainScreenObject;

        public MainSteps(IMainScreenObject mainScreenObject)
        {
            _mainScreenObject = mainScreenObject;
        }

        public void ThenApplicationNavigatesToTheMainScreen()
        {
            var isActive = _mainScreenObject.IsActive();
            isActive.Should().BeTrue();
        }
    }
}