using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LogoFX.Core;
using MinimalisticWpf.Client.Data.Contracts.Providers;
using MinimalisticWpf.Client.Model.Contracts;

namespace MinimalisticWpf.Client.Model
{
    [UsedImplicitly]
    public sealed class DataService : IDataService
    {
        private readonly IWarehouseProvider _warehouseProvider;

        public DataService(IWarehouseProvider warehouseProvider)
        {
            _warehouseProvider = warehouseProvider;
        }

        private readonly RangeObservableCollection<IWarehouseItem> _warehouseItems =
            new RangeObservableCollection<IWarehouseItem>();

        IEnumerable<IWarehouseItem> IDataService.WarehouseItems
        {
            get { return _warehouseItems; }
        }

        public async Task GetWarehouseItemsAsync()
        {
            var warehouseItems = await Task.Run(() =>
            {
                return _warehouseProvider
                    .GetWarehouseItems()
                    .Select(x => new WarehouseItem(x.Kind, x.Price, x.Quantity));
            });

            _warehouseItems.Clear();
            _warehouseItems.AddRange(warehouseItems);
        }
    }
}