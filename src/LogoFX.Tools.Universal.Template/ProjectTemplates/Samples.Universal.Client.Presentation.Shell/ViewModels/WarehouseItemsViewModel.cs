using System.Collections;
using Caliburn.Micro;
using JetBrains.Annotations;
using LogoFX.Client.Mvvm.ViewModel;
using $saferootprojectname$.Client.Model.Contracts;

namespace $safeprojectname$.ViewModels
{
    [UsedImplicitly]
    public class WarehouseItemsViewModel : PropertyChangedBase
    {
        private readonly IDataService _dataService;        

        public WarehouseItemsViewModel(
            IDataService dataService)
        {
            _dataService = dataService;            
        }

        private IEnumerable _warehouseItems;
        public IEnumerable WarehouseItems
        {
            get { return _warehouseItems ?? (_warehouseItems = CreateWarehouseItems()); }
        }

        private IEnumerable CreateWarehouseItems()
        {
            var wc = new WrappingCollection
            {
                FactoryMethod =
                    o =>
                        new WarehouseItemViewModel((IWarehouseItem)o)
            }.WithSource(_dataService.WarehouseItems);

            return wc.AsView();
        }
    }
}
