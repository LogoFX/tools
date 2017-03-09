using System;
using System.Collections.Generic;
using $saferootprojectname$.Client.Data.Contracts.Dto;
using $saferootprojectname$.Client.Data.Contracts.Providers;

namespace $safeprojectname$
{
    class WarehouseProvider : IWarehouseProvider
    {
        public IEnumerable<WarehouseItemDto> GetWarehouseItems()
        {
            //put here real data logic
            return (IEnumerable<WarehouseItemDto>) new[]
            {
                new WarehouseItemDto
                {
                    Kind = "Acme",
                    Price = 10,
                    Quantity = 10
                },

                new WarehouseItemDto
                {
                    Kind = "Bacme",
                    Price = 20,
                    Quantity = 3
                },

                new WarehouseItemDto
                {
                    Kind = "Exceed",
                    Price = 0.4,
                    Quantity = 100
                },

                new WarehouseItemDto
                {
                    Kind = "Acme2",
                    Price = 1,
                    Quantity = 10
                },
            };           
        }

        public bool DeleteWarehouseItem(Guid id)
        {
            throw new NotImplementedException();
        }

        public void SaveWarehouseItem(WarehouseItemDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
