using System;
using System.Collections.Generic;
using Attest.Fake.Setup.Contracts;
using LogoFX.Client.Data.Fake.ProviderBuilders;
using $saferootprojectname$.Client.Data.Contracts.Providers;
using Attest.Fake.Core;

namespace $safeprojectname$
{    
    public class LoginProviderBuilder : FakeBuilderBase<ILoginProvider>
    {        
        private readonly Dictionary<string, string> _users = new Dictionary<string, string>();
        
        private LoginProviderBuilder()
        {

        }

        public static LoginProviderBuilder CreateBuilder()
        {
            return new LoginProviderBuilder();
        }

        public void WithUser(string username, string password)
        {
            _users.Add(username, password);            
        }

        protected override IServiceCall<ILoginProvider> CreateServiceCall(IHaveNoMethods<ILoginProvider> serviceCallTemplate)
        {
            var setup = serviceCallTemplate
               .AddMethodCall<string, string>(t => t.Login(It.IsAny<string>(), It.IsAny<string>()),
                    (r, login, password) => _users.ContainsKey(login)
                        ? _users[login] == password
                            ? r.Complete()
                            : r.Throw(new Exception("Unable to login."))
                        : r.Throw(new Exception("Login not found.")));
            return setup;
        }
    }
}