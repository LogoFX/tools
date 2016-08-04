#if FAKE
using LogoFX.Client.Testing.Contracts;
using $saferootprojectname$.Client.Data.Fake.ProviderBuilders;
#endif

#if REAL
#endif

namespace $safeprojectname$
{
    public class GivenLoginSteps
    {
#if FAKE
        private readonly IBuilderRegistrationService _builderRegistrationService;
        private readonly LoginProviderBuilder _loginProviderBuilder;

        public GivenLoginSteps(IBuilderRegistrationService builderRegistrationService, 
            LoginProviderBuilder loginProviderBuilder)
        {
            _builderRegistrationService = builderRegistrationService;
            _loginProviderBuilder = loginProviderBuilder;
        }
#endif

        public void SetupAuthenticatedUserWithCredentials(string username, string password)
        {
#if FAKE
            _loginProviderBuilder.WithUser(username, password);
            _builderRegistrationService.RegisterBuilder(_loginProviderBuilder);
#endif

#if REAL
            //put here real Setup
#endif
        }

        public void SetupLoginSuccessfullyWithUsername(string username)
        {
#if FAKE
            _loginProviderBuilder.WithSuccessfulLogin(username);
            _builderRegistrationService.RegisterBuilder(_loginProviderBuilder);
#endif

#if REAL
            //put here real Setup
#endif
        }
    }
}
