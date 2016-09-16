using System.Threading.Tasks;
using $saferootprojectname$.Client.Data.Contracts.Providers;

namespace $safeprojectname$
{
    class LoginProvider : ILoginProvider
    {
        public Task Login(string username, string password)
        {
            return Task.Run(() =>
            {
                // TODO: Add login logic here
            });
        }
    }
}