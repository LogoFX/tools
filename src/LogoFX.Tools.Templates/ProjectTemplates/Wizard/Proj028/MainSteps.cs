using FluentAssertions;
using $saferootprojectname$.Tests.Domain.ScreenObjects;

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
