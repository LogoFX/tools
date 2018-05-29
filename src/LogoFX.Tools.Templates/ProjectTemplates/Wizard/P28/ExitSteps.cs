using FluentAssertions;
using $saferootprojectname$.Tests.Domain.ScreenObjects;

namespace $safeprojectname$
{
    public class ExitSteps
    {
        private readonly IExitScreenObject _exitScreenObject;

        public ExitSteps(IExitScreenObject exitScreenObject)
        {
            _exitScreenObject = exitScreenObject;
        }

        public void ThenTheExitApplicationOptionsDisplayStatusIs(bool status)
        {
            var isDisplayed = _exitScreenObject.IsDisplayed();
            isDisplayed.Should().Be(status);
        }

        public void WhenISelectExitWithSaveOption()
        {
            _exitScreenObject.ExitWithSave();            
        }

        public void WhenISelectExitWithoutSaveOption()
        {
            _exitScreenObject.ExitWithoutSave();
        }

        public void WhenISelectCancelOption()
        {
            _exitScreenObject.Cancel();
        }
    }
}