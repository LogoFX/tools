using $saferootprojectname$.pecifications.Client.Tests.Integration.Infra.Core;
using $saferootprojectname$.pecifications.Tests.Domain.ScreenObjects;

namespace $safeprojectname$.ScreenObjects
{
    class LoginScreenObject : ILoginScreenObject
    {
        public StructureHelper StructureHelper { get; }

        public LoginScreenObject(StructureHelper structureHelper)
        {
            StructureHelper = structureHelper;
        }

        public bool IsActive()
        {
            var loginViewModel = StructureHelper.GetLogin();
            return loginViewModel.IsActive;
        }

        public void SetUsername(string username)
        {
            var loginViewModel = StructureHelper.GetLogin();
            loginViewModel.UserName = username;
        }

        public void SetPassword(string password)
        {
            var loginViewModel = StructureHelper.GetLogin();
            loginViewModel.Password = password;
        }

        public void Login()
        {
            var loginViewModel = StructureHelper.GetLogin();
            loginViewModel.LoginCommand.Execute(null);
        }

        public string GetErrorMessage()
        {
            var loginViewModel = StructureHelper.GetLogin();
            return loginViewModel.LoginFailureCause;
        }
    }
}
