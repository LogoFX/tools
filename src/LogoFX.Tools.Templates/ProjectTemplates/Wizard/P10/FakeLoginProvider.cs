using Attest.Fake.Builders;
using JetBrains.Annotations;
using $saferootprojectname$.Client.Data.Contracts.Providers;
using $saferootprojectname$.Client.Data.Fake.Containers;
using $saferootprojectname$.Client.Data.Fake.ProviderBuilders;

namespace $safeprojectname$
{
    [UsedImplicitly]
    class FakeLoginProvider : FakeProviderBase<LoginProviderBuilder, ILoginProvider>, ILoginProvider
    {
        private readonly LoginProviderBuilder _loginProviderBuilder;

        public FakeLoginProvider(
            LoginProviderBuilder loginProviderBuilder,
            IUserContainer userContainer)
        {
            _loginProviderBuilder = loginProviderBuilder;
            foreach (var user in userContainer.Users)
            {
                _loginProviderBuilder.WithUser(user.Item1, user.Item2);
            }            
        }

        void ILoginProvider.Login(string username, string password)
        {
            var service = GetService(() => _loginProviderBuilder, b => b);
            service.Login(username, password);
        }
    }
}