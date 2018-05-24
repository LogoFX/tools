namespace $safeprojectname$
{
    internal interface IExecutableContainer
    {
        string Path { get; }
    }

    class ExecutableContainer : IExecutableContainer
    {
        public string Path => "$saferootprojectname$.pecifications.Client.Launcher.exe";
    }
}