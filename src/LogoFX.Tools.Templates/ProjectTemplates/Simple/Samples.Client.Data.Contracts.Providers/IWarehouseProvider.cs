using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using $saferootprojectname$.Client.Data.Contracts.Dto;

namespace $safeprojectname$
{
    public interface IWarehouseProvider
    {
        Task<IEnumerable<WarehouseItemDto>> GetWarehouseItems();
        Task<bool> DeleteWarehouseItem(Guid id);
        Task SaveWarehouseItem(WarehouseItemDto dto);
    }
}
