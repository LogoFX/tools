using System;
using System.Collections.Generic;
using System.Linq;
using RestSharp;
using $saferootprojectname$.lient.Data.Contracts.Dto;
using $saferootprojectname$.lient.Data.Contracts.Providers;

namespace $safeprojectname$
{
    class LoginProvider : ILoginProvider
    {        
        private readonly RestClient _client;

        public LoginProvider(RestClient client)
        {
            _client = client;
        }

        public void Login(string username, string password)
        {
            //TODO: very naive ))
            var restRequest = new RestRequest("api/user", Method.GET) { RequestFormat = DataFormat.Json };
            IRestResponse<List<UserDto>> response;
            try
            {
                response = _client.Execute<List<UserDto>>(restRequest);                
            }
            catch (Exception e)
            {                
                throw new Exception("Unable to login.");
            }

            var matchingUser = response.Data?.SingleOrDefault(t => t.Login == username);
            if (matchingUser == null)
            {
                throw new Exception("Login not found.");
            }

            if (matchingUser.Password != password)
            {
                throw new Exception("Unable to login.");
            }           
        }
    }
}