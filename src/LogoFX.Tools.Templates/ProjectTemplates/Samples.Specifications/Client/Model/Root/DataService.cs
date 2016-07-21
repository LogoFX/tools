using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LogoFX.Core;
using Samples.Client.Data.Contracts.Providers;
using $safeprojectname$.Contracts;
using $safeprojectname$.Mappers;

namespace $safeprojectname$
{
    [UsedImplicitly]
    class DataService : IDataService
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

        private async Task GetWarehouseItemsInternal()
        {
            var warehouseItems =
                (await _warehouseProvider.GetWarehouseItems()).Select(WarehouseMapper.MapToWarehouseItem);
            _warehouseItems.Clear();
            _warehouseItems.AddRange(warehouseItems);
        }
    }
}
