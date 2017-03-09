using LogoFX.Client.Testing.EndToEnd.White;
using $saferootprojectname$.Tests.Domain.ScreenObjects;
using TestStack.White.UIItems;

namespace $safeprojectname$.ScreenObjects
{
    class ExitScreenObject : IExitScreenObject
    {
        public bool IsDisplayed()
        {
            var application = ApplicationContext.Application;
            var exitWindow = application?.GetWindowEx("Exit options");
            return exitWindow?.Visible ?? false;
        }

        public void ExitWithSave()
        {
            var application = ApplicationContext.Application;
            var exitWindow = application?.GetWindowEx("Exit options");

            var exitControl = exitWindow?.Get<Button>("ExitWithSave");
            exitControl?.Click();
        }

        public void ExitWithoutSave()
        {
            var application = ApplicationContext.Application;
            var exitWindow = application?.GetWindowEx("Exit options");

            var exitControl = exitWindow?.Get<Button>("ExitWithoutSave");
            exitControl?.Click();
        }

        public void Cancel()
        {
            var application = ApplicationContext.Application;
            var exitWindow = application?.GetWindowEx("Exit options");

            var exitControl = exitWindow?.Get<Button>("ExitCancel");
            exitControl?.Click();
        }
    }
}