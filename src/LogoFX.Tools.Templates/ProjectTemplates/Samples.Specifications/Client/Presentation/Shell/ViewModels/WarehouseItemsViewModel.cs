using System.Collections;
using System.ComponentModel;
using Caliburn.Micro;
using JetBrains.Annotations;
using LogoFX.Client.Mvvm.ViewModel;
using LogoFX.Client.Mvvm.ViewModel.Services;
using $saferootprojectname$.Client.Model.Contracts;

namespace $safeprojectname$.ViewModels
{
    [UsedImplicitly]
    public class WarehouseItemsViewModel : PropertyChangedBase
    {
        private readonly IDataService _dataService;
        private readonly IViewModelCreatorService _viewModelCreatorService;

        public WarehouseItemsViewModel(
            IDataService dataService,
            IViewModelCreatorService viewModelCreatorService)
        {
            _dataService = dataService;
            _viewModelCreatorService = viewModelCreatorService;
        }

        private ICollectionView _warehouseItems;
        public IEnumerable WarehouseItems
        {
            get { return _warehouseItems ?? (_warehouseItems = CreateWarehouseItems()); }
        }

        private ICollectionView CreateWarehouseItems()
        {
            var wc = new WrappingCollection
            {
                FactoryMethod =
                    o =>
                        _viewModelCreatorService.CreateViewModel<IWarehouseItem, WarehouseItemViewModel>(
                            (IWarehouseItem) o)
            }.WithSource(_dataService.WarehouseItems);

            return wc.AsView();
        }
    }
}
