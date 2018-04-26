using System;
using System.Collections.Generic;
using $saferootprojectname$.Client.Data.Contracts.Dto;

namespace $safeprojectname$
{
    public interface IWarehouseProvider
    {
        IEnumerable<WarehouseItemDto> GetWarehouseItems();
        bool DeleteWarehouseItem(Guid id);
        void SaveWarehouseItem(WarehouseItemDto dto);        
    }
}
