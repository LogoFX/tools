using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using JetBrains.Annotations;
using LogoFX.Client.Mvvm.Commanding;
using LogoFX.Client.Mvvm.ViewModel.Extensions;
using LogoFX.Client.Mvvm.ViewModel.Services;
using LogoFX.Client.Mvvm.ViewModel.Shared;
using LogoFX.Core;
using $saferootprojectname$.Client.Model.Contracts;

namespace $safeprojectname$.ViewModels
{
    [UsedImplicitly]
    public class MainViewModel : BusyScreen
    {
        private readonly IViewModelCreatorService _viewModelCreatorService;
        private readonly IDataService _dataService;
        private readonly IWindowManager _windowManager;

        public MainViewModel(
            IViewModelCreatorService viewModelCreatorService,
            IDataService dataService,            
            IWindowManager windowManager)
        {
            _viewModelCreatorService = viewModelCreatorService;
            _dataService = dataService;
            _windowManager = windowManager;

            NewWarehouseItem();
        }

        private ICommand _newCommand;
        public ICommand NewCommand
        {
            get
            {
                return _newCommand ?? (_newCommand = ActionCommand.Do(NewWarehouseItem));
            }
        }

        private ICommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                return _deleteCommand ??
                       (_deleteCommand = ActionCommand
                           .When(() => ActiveWarehouseItem?.Item.Model.IsNew == false)
                           .Do(DeleteSelectedItem)
                           .RequeryOnPropertyChanged(this, () => ActiveWarehouseItem));
            }
        }

        private WarehouseItemContainerViewModel _activeWarehouseItem;
        public WarehouseItemContainerViewModel ActiveWarehouseItem
        {
            get { return _activeWarehouseItem; }
            set
            {
                if (_activeWarehouseItem == value)
                {
                    return;
                }

                if (_activeWarehouseItem != null)
                {
                    _activeWarehouseItem.Saving -= OnSaving;
                    _activeWarehouseItem.Saved -= OnSaved;
                }

                _activeWarehouseItem = value;

                if (_activeWarehouseItem != null)
                {
                    _activeWarehouseItem.Saving += OnSaving;
                    _activeWarehouseItem.Saved += OnSaved;
                }

                NotifyOfPropertyChange();
            }
        }

        private async void OnSaved(object sender, ResultEventArgs e)
        {
            IsBusy = true;
            try
            {
                await _dataService.GetWarehouseItemsAsync();
            }

            finally
            {
                IsBusy = false;
            }
            NewWarehouseItem();
        }

        private void OnSaving(object sender, EventArgs e)
        {
            IsBusy = true;
        }

        private WarehouseItemsViewModel _warehouseItems;
        public WarehouseItemsViewModel WarehouseItems
        {
            get { return _warehouseItems ?? (_warehouseItems = CreateWarehouseItems()); }
        }

        private WarehouseItemsViewModel CreateWarehouseItems()
        {
            var warehouseItemsViewModel = _viewModelCreatorService.CreateViewModel<WarehouseItemsViewModel>();
            EventHandler strongHandler = WarehouseItemsSelectionChanged;
            warehouseItemsViewModel.Items.SelectionChanged += WeakDelegate.From(strongHandler);
            return warehouseItemsViewModel;
        }

        private void WarehouseItemsSelectionChanged(object sender, EventArgs e)
        {
            var selectedItem = WarehouseItems.Items.SelectedItem;
            ActiveWarehouseItem = selectedItem == null ? null :
                _viewModelCreatorService.CreateViewModel<IWarehouseItem, WarehouseItemContainerViewModel>(
                    ((WarehouseItemViewModel) WarehouseItems.Items.SelectedItem).Model);
        }

        private EventsViewModel _events;
        public EventsViewModel Events
        {
            get { return _events ?? (_events = _viewModelCreatorService.CreateViewModel<EventsViewModel>()); }
        }        

        private async void NewWarehouseItem()
        {            
            IsBusy = true;

            try
            {
                var warehouseItem = await _dataService.NewWarehouseItemAsync();
                var newItem = _viewModelCreatorService.CreateViewModel<IWarehouseItem, WarehouseItemContainerViewModel>(warehouseItem);
                ActiveWarehouseItem = newItem;
            }

            finally
            {
                IsBusy = false;
            }
        }

        private async void DeleteSelectedItem()
        {            
            IsBusy = true;

            try
            {
                await _dataService.DeleteWarehouseItemAsync(ActiveWarehouseItem?.Item.Model);
            }

            finally
            {
                IsBusy = false;
            }

            NewWarehouseItem();
        }

        protected override async void OnInitialize()
        {
            base.OnInitialize();
            await _dataService.GetWarehouseItemsAsync();
        }

        public override async void CanClose(Action<bool> callback)
        {
            if (_dataService.WarehouseItems.Any(t => t.IsDirty))
            {
                var exitOptionsViewModel = _viewModelCreatorService.CreateViewModel<ExitOptionsViewModel>();
                _windowManager.ShowDialog(exitOptionsViewModel);
                var result = exitOptionsViewModel.Result;
                if (result == MessageResult.Yes)
                {
                    foreach (var warehouseItem in _dataService.WarehouseItems.Where(t => t.IsDirty && t.CanCommitChanges))
                    {                        
                        await _dataService.SaveWarehouseItemAsync(warehouseItem);
                        warehouseItem.CommitChanges();
                    }
                    await WaitForTestApplication();
                    callback(true);
                }
                else if (result == MessageResult.No)
                {
                    foreach (var warehouseItem in _dataService.WarehouseItems.Where(t => t.IsDirty && t.CanCancelChanges))
                    {
                        warehouseItem.CancelChanges();                        
                    }
                    await WaitForTestApplication();
                    callback(true);
                }
                else if (result == MessageResult.Cancel)
                {
                    callback(false);
                }                
            }
            else
            {
                callback(true);
            }
        }

        private static async Task WaitForTestApplication()
        {
            //Added for testability purposes only
            //The UI test engine has to query controls and perform several actions
            await Task.Delay(1000);
        }
    }
}