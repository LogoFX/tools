using System.Collections.Generic;
using $saferootprojectname$.Client.Data.Contracts.Dto;

namespace $safeprojectname$
{
    public interface IWarehouseProvider
    {
        IEnumerable<WarehouseItemDto> GetWarehouseItems();
    }
}