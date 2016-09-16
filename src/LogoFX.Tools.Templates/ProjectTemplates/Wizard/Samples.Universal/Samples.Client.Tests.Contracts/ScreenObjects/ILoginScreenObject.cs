namespace $safeprojectname$.ScreenObjects
{
    public interface ILoginScreenObject
    {
        bool IsActive();
        void SetUsername(string username);
        void Login();
    }
}
