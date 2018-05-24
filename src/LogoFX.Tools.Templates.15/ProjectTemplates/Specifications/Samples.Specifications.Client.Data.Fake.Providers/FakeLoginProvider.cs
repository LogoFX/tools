using Attest.Fake.Builders;
using JetBrains.Annotations;
using $saferootprojectname$.lient.Data.Contracts.Providers;
using $saferootprojectname$.pecifications.Client.Data.Fake.Containers;
using $saferootprojectname$.pecifications.Client.Data.Fake.ProviderBuilders;

namespace $safeprojectname$
{
    [UsedImplicitly]
    class FakeLoginProvider : FakeProviderBase<LoginProviderBuilder, ILoginProvider>, ILoginProvider
    {
        public FakeLoginProvider(
            LoginProviderBuilder loginProviderBuilder,
            IUserContainer userContainer)
            :base(loginProviderBuilder)
        {
            foreach (var user in userContainer.Users)
            {
                loginProviderBuilder.WithUser(user.Item1, user.Item2);
            }
        }

        void ILoginProvider.Login(string username, string password)
        {
            var service = GetService();
            service.Login(username, password);
        }
    }
}