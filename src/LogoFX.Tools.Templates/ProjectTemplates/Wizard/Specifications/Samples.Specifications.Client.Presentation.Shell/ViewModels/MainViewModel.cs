using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using JetBrains.Annotations;
using LogoFX.Client.Mvvm.Commanding;
using LogoFX.Client.Mvvm.ViewModel.Extensions;
using LogoFX.Client.Mvvm.ViewModel.Services;
using $saferootprojectname$.Client.Model.Contracts;

namespace $safeprojectname$.ViewModels
{
    [UsedImplicitly]
    public class MainViewModel : BusyScreen
    {
        private readonly IViewModelCreatorService _viewModelCreatorService;
        private readonly IDataService _dataService;

        public MainViewModel(
            IViewModelCreatorService viewModelCreatorService,
            IDataService dataService)
        {
            _viewModelCreatorService = viewModelCreatorService;
            _dataService = dataService;

            ActiveWarehouseItem = _viewModelCreatorService.CreateViewModel<WarehouseItemViewModel>();
        }

        private ICommand _newCommand;

        public ICommand NewCommand
        {
            get
            {
                return _newCommand ??
                       (_newCommand = ActionCommand
                           .When(() => true)
                           .Do(() => { }));
            }
        }

        private ICommand _applyCommand;

        public ICommand ApplyCommand
        {
            get
            {
                return _applyCommand ??
                       (_applyCommand = ActionCommand
                           .When(() => true)
                           .Do(() => { }));
            }
        }

        private ICommand _discardCommand;

        public ICommand DiscardCommand
        {
            get
            {
                return _discardCommand ??
                       (_discardCommand = ActionCommand
                           .When(() => true)
                           .Do(() => { }));
            }
        }

        private WarehouseItemViewModel _activeWarehouseItem;

        public WarehouseItemViewModel ActiveWarehouseItem
        {
            get { return _activeWarehouseItem; }
            set
            {
                if (_activeWarehouseItem == value)
                {
                    return;
                }

                _activeWarehouseItem = value;
                NotifyOfPropertyChange();
            }
        }

        private WarehouseItemsViewModel _warehouseItems;
        public WarehouseItemsViewModel WarehouseItems
        {
            get { return _warehouseItems ?? (_warehouseItems = CreateViewModel()); }
        }

        private WarehouseItemsViewModel CreateViewModel()
        {
            var warehouseItemsViewModel = _viewModelCreatorService.CreateViewModel<WarehouseItemsViewModel>();
            warehouseItemsViewModel.WarehouseItems.SelectionChanged += WarehouseItems_SelectionChanged;
            return warehouseItemsViewModel;
        }

        private void WarehouseItems_SelectionChanged(object sender, System.EventArgs e)
        {
            ActiveWarehouseItem = (WarehouseItemViewModel) WarehouseItems.WarehouseItems.SelectedItem;
        }

        private async Task NewwarehouseItem()
        {
            Debug.Assert(!IsBusy);

            IsBusy = true;

            try
            {
                var warehouseItem = await _dataService.NewWarehouseItemAsync();
                ActiveWarehouseItem = new WarehouseItemViewModel(warehouseItem);                
            }

            finally
            {
                IsBusy = false;
            }
        }

        protected override async void OnInitialize()
        {
            base.OnInitialize();

            await _dataService.GetWarehouseItemsAsync();
            //await NewwarehouseItem();
        }
    }
}