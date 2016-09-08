using JetBrains.Annotations;
using MinimalisticWpf.Client.Data.Contracts.Providers;

namespace MinimalisticWpf.Client.Data.Real.Providers
{
    [UsedImplicitly]
    public sealed class LoginProvider : ILoginProvider
    {
        public void Login(string username, string password)
        {
            // TODO: Add login logic here
        }
    }
}