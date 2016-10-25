using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LogoFX.Core;
using $saferootprojectname$.Client.Data.Contracts.Providers;
using $safeprojectname$.Contracts;
using $safeprojectname$.Mappers;

namespace $safeprojectname$
{
    [UsedImplicitly]
    internal sealed class DataService : IDataService
    {
        private readonly IWarehouseProvider _warehouseProvider;        

        public DataService(IWarehouseProvider warehouseProvider)
        {
            _warehouseProvider = warehouseProvider;
        }

        private readonly RangeObservableCollection<IWarehouseItem> _warehouseItems = new RangeObservableCollection<IWarehouseItem>();
        IEnumerable<IWarehouseItem> IDataService.WarehouseItems
        {
            get { return _warehouseItems; }
        }

        public async Task GetWarehouseItemsAsync()
        {
            await ServiceRunner.RunAsync(GetWarehouseItemsInternal);
        }

        public async Task<IWarehouseItem> NewWarehouseItemAsync()
        {
            await Task.Delay(1000);
            return new WarehouseItem("", 0d, 1);
        }

        private async Task GetWarehouseItemsInternal()
        {
            var warehouseItems =
                (await _warehouseProvider.GetWarehouseItems()).Select(WarehouseMapper.MapToWarehouseItem);
            _warehouseItems.Clear();
            _warehouseItems.AddRange(warehouseItems);
        }
    }
}
