using System.Collections.Generic;
using System.Threading.Tasks;
using $saferootprojectname$.Client.Data.Contracts.Dto;
using $saferootprojectname$.Client.Data.Contracts.Providers;

namespace $safeprojectname$
{
    class WarehouseProvider : IWarehouseProvider
    {
        public Task<IEnumerable<WarehouseItemDto>> GetWarehouseItems()
        {
            //put here real data logic
            return Task.FromResult((IEnumerable<WarehouseItemDto>) new[]
            {
                new WarehouseItemDto
                {
                    Kind = "Acme",
                    Price = 10,
                    Quantity = 10
                }
            });           
        }
    }
}
