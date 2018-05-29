using System.Threading.Tasks;
using JetBrains.Annotations;
using $saferootprojectname$.Client.Data.Contracts.Providers;
using $safeprojectname$.Contracts;
using $safeprojectname$.Shared;

namespace $safeprojectname$
{
    [UsedImplicitly]
    class LoginService : ILoginService
    {
        private readonly ILoginProvider _loginProvider;

        public LoginService(ILoginProvider loginProvider)
        {
            _loginProvider = loginProvider;
        }

        public async Task LoginAsync(string username, string password)
        {
            await ServiceRunner.RunAsync(() => LoginInternal(username, password));
        }

        private async Task LoginInternal(string username, string password)
        {
            await ServiceRunner.RunAsync(() =>
            {
                _loginProvider.Login(username, password);
                UserContext.Current = new User(username);
            });
        }
    }
}