using System.IO;
using $saferootprojectname$.lient.Data.Contracts.Dto;
using $saferootprojectname$.pecifications.Server.Storage.NDatabase;

namespace $safeprojectname$
{
    public class NDatabaseSetupHelper : ISetupHelper
    {
        public void AddWarehouseItem(WarehouseItemDto warehouseItem)
        {
            using (var storage = new Storage())
            {
                storage.Store(warehouseItem);
            }
        }

        public void AddUser(string login, string password)
        {
            using (var storage = new Storage())
            {
                storage.Store(new UserDto
                {
                    Login = login,
                    Password = password
                });
            }
        }        

        public void Initialize()
        {
            File.Delete("objects.ndb");
        }
    }    
}