namespace $safeprojectname$.ScreenObjects
{
    public interface IExitScreenObject
    {
        bool IsDisplayed();
        void ExitWithSave();
        void ExitWithoutSave();
        void Cancel();
    }
}