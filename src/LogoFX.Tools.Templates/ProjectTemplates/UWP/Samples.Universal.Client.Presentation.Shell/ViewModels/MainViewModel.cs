using Caliburn.Micro;
using JetBrains.Annotations;
using $saferootprojectname$.Client.Model.Contracts;

namespace $safeprojectname$.ViewModels
{
    [UsedImplicitly]
    public class MainViewModel : Screen
    {
        private readonly IDataService _dataService;

        public MainViewModel(            
            IDataService dataService)
        {
            _dataService = dataService;
        }

        private WarehouseItemsViewModel _warehouseItems;
        public WarehouseItemsViewModel WarehouseItems
        {
            get { return _warehouseItems ?? (_warehouseItems = new WarehouseItemsViewModel(_dataService)); }
        }

        protected override async void OnInitialize()
        {
            base.OnInitialize();
            await _dataService.GetWarehouseItemsAsync();
        }
    }
}