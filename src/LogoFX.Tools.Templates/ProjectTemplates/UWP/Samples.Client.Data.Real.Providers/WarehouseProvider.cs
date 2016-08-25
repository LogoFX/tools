using System.Collections.Generic;
using System.Threading.Tasks;
using $saferootprojectname$.Client.Data.Contracts.Dto;
using $saferootprojectname$.Client.Data.Contracts.Providers;

namespace $safeprojectname$
{
    class WarehouseProvider : IWarehouseProvider
    {
        public async Task<IEnumerable<WarehouseItemDto>> GetWarehouseItems()
        {
            return await Task.Run(() => new[]
            {
                new WarehouseItemDto
                {
                    Kind = "Good"
                }
            });
        }
    }
}