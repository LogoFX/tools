using System;
using System.Collections.Generic;
using Attest.Fake.Core;
using Attest.Fake.Setup.Contracts;
using LogoFX.Client.Data.Fake.ProviderBuilders;
using $saferootprojectname$.Client.Data.Contracts.Providers;

namespace $safeprojectname$
{    
    public class LoginProviderBuilder : FakeBuilderBase<ILoginProvider>
    {        
        private readonly List<Tuple<string, string>> _users = new List<Tuple<string, string>>();
        private readonly Dictionary<string, bool> _isLoginAttemptSuccessfulCollection = new Dictionary<string, bool>();
        
        private LoginProviderBuilder()
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

        protected override IServiceCall<ILoginProvider> CreateServiceCall(IHaveNoMethods<ILoginProvider> serviceCallTemplate)
        {
            var setup = serviceCallTemplate
               .AddMethodCallAsync<string, string>(t => t.Login(It.IsAny<string>(), It.IsAny<string>()),
                    (r, login, password) =>
                           _isLoginAttemptSuccessfulCollection.ContainsKey(login)
                               ? _isLoginAttemptSuccessfulCollection[login]
                                   ? r.Complete()
                                   : r.Throw(new Exception("unable to login"))
                               : r.Throw(new Exception("unable to login")));
            return setup;
        }

        public void WithSuccessfulLogin(string username)
        {
            _isLoginAttemptSuccessfulCollection[username] = true;
        }
    }
}