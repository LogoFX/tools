using AutoMapper;
using $saferootprojectname$.Client.Data.Contracts.Dto;
using $safeprojectname$.Contracts;

namespace $safeprojectname$.Mappers
{
    static class WarehouseMapper
    {
        internal static IWarehouseItem MapToWarehouseItem(WarehouseItemDto warehouseItemDto)
        {
            return Mapper.Map<WarehouseItem>(warehouseItemDto);
        }
    }
}
