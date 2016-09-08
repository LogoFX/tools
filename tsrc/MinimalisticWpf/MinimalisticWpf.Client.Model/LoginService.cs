using System.Threading.Tasks;
using JetBrains.Annotations;
using MinimalisticWpf.Client.Data.Contracts.Providers;
using MinimalisticWpf.Client.Model.Contracts;
using MinimalisticWpf.Client.Model.Shared;

namespace MinimalisticWpf.Client.Model
{
    [UsedImplicitly]
    public sealed class LoginService : ILoginService
    {
        private readonly ILoginProvider _loginProvider;

        public LoginService(ILoginProvider loginProvider)
        {
            _loginProvider = loginProvider;
        }

        public async Task LoginAsync(string username, string password)
        {
            await Task.Run(() =>
            {
                _loginProvider.Login(username, password);
            });
            UserContext.Current = new User(username);
        }
    }
}