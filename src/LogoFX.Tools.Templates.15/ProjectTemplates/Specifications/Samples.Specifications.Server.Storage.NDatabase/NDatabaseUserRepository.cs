using System.Collections.Generic;
using System.Linq;
using $saferootprojectname$.lient.Data.Contracts.Dto;
using $saferootprojectname$.pecifications.Server.Domain.Models;
using $saferootprojectname$.pecifications.Server.Domain.Services.Storage;

namespace $safeprojectname$
{
    //TODO: not operable yet
    class NDatabaseUserRepository : IUserRepository
    {
        public IEnumerable<User> GetAll()
        {
            using (var storage = new Storage())
            {
                return storage.Get<UserDto>().Select(t => new User
                {
                    Login = t.Login,
                    Password = t.Password
                });
            }
        }
    }
}
