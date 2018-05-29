using AutoMapper;
using $saferootprojectname$.Client.Data.Contracts.Dto;
using $safeprojectname$.Contracts;

namespace $safeprojectname$.Mappers
{
    static class WarehouseMapper
    {
        internal static IWarehouseItem MapToWarehouseItem(WarehouseItemDto warehouseItemDto)
        {
            var item = Mapper.Map<WarehouseItem>(warehouseItemDto);
            return item;
        }

        internal static WarehouseItemDto MapToWarehouseDto(IWarehouseItem warehouseItem)
        {
            return Mapper.Map<WarehouseItemDto>(warehouseItem);
        }
    }
}
