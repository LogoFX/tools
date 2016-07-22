using System.Collections.Generic;
using $saferootprojectname$.Client.Data.Contracts.Dto;

namespace $safeprojectname$
{
    public interface IWarehouseContainer : IDataContainer
    {
        IEnumerable<WarehouseItemDto> WarehouseItems { get; }
    }

    public class WarehouseContainer : IWarehouseContainer
    {
        private readonly List<WarehouseItemDto> _warehouseItems = new List<WarehouseItemDto>();

        public IEnumerable<WarehouseItemDto> WarehouseItems
        {
            get { return _warehouseItems; }
        }

        public void UpdateWarehouseItems(IEnumerable<WarehouseItemDto> warehouseItems)
        {
            _warehouseItems.Clear();
            _warehouseItems.AddRange(warehouseItems);
        }
    }   
}
