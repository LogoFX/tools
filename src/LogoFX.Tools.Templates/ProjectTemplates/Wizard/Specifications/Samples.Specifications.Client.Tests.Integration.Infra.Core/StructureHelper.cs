using $saferootprojectname$.Client.Presentation.Shell.ViewModels;

namespace $safeprojectname$
{
    /// <summary>
    /// Represents structure helper which provides easier API for accessing different parts of application
    /// </summary>
    public class StructureHelper
    {
        private ShellViewModel _rootObject;

        /// <summary>
        /// Sets the root object which is the shell view model.
        /// </summary>
        /// <param name="rootObject">The root object.</param>
        public void SetRootObject(ShellViewModel rootObject)
        {
            _rootObject = rootObject;
        }

        /// <summary>
        /// Gets the shell view model.
        /// </summary>
        /// <returns>Shell view model</returns>
        public ShellViewModel GetShell()
        {
            return GetShellInternal();
        }

        public LoginViewModel GetLogin()
        {
            return GetShellInternal()?.LoginViewModel;
        }

        public MainViewModel GetMain()
        {
            return GetShellInternal()?.MainViewModel;
        }

        private ShellViewModel GetShellInternal()
        {
            return _rootObject;
        }
    }
}
