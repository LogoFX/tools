using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Attest.Fake.Builders;
using Attest.Fake.LightMock;
using Attest.Fake.Setup;
using LightMock;
using $saferootprojectname$.Client.Data.Contracts.Providers;

namespace $safeprojectname$
{               
    public class LoginProviderBuilder : FakeBuilderBase<ILoginProvider>
    {
        class LoginProviderProxy : ProviderProxyBase<ILoginProvider>, ILoginProvider
        {
            public LoginProviderProxy(IInvocationContext<ILoginProvider> context)
                : base(context)
            {
            }

            public Task Login(string username, string password)
            {
                return Invoke(t => t.Login(username, password));
            }
        }

        private readonly List<Tuple<string, string>> _users = new List<Tuple<string, string>>();
        private readonly Dictionary<string, bool> _isLoginAttemptSuccessfulCollection = new Dictionary<string, bool>();
        
        private LoginProviderBuilder()
            :base(FakeFactoryHelper.CreateFake<ILoginProvider>(c => new LoginProviderProxy(c)))
        {

        }

        public static LoginProviderBuilder CreateBuilder()
        {
            return new LoginProviderBuilder();
        }

        public void WithUser(string username, string password)
        {
            _users.Add(new Tuple<string, string>(username, password));            
        }

        protected override void SetupFake()
        {            
            var initialSetup = ServiceCallFactory.CreateServiceCall(FakeService);

            var setup = initialSetup
               .AddMethodCallAsync<string, string>(t => t.Login(The<string>.IsAnyValue, The<string>.IsAnyValue),
                    (r, login, password) =>
                           _isLoginAttemptSuccessfulCollection.ContainsKey(login)
                               ? _isLoginAttemptSuccessfulCollection[login]
                                   ? r.Complete()
                                   : r.Throw(new Exception("unable to login"))
                               : r.Throw(new Exception("unable to login")));           

            setup.Build();
        }

        public void WithSuccessfulLogin(string username)
        {
            _isLoginAttemptSuccessfulCollection[username] = true;
        }
    }
}