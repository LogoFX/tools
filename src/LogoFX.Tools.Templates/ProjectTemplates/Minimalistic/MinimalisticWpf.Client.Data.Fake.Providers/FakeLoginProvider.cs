using System.Security;
using JetBrains.Annotations;
using $saferootprojectname$.Client.Data.Contracts.Providers;

namespace $safeprojectname$
{
    [UsedImplicitly]
    public sealed class FakeLoginProvider : ILoginProvider
    {
        public void Login(string username, string password)
        {
            if (username == "admin" && password == "123")
            {
                return;
            }

            throw new SecurityException("Wrong name or password.");
        }
    }
}