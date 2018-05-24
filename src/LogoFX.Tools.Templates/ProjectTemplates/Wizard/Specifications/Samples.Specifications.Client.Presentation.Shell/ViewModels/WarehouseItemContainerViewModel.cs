using System.Threading.Tasks;
using LogoFX.Client.Mvvm.ViewModel.Extensions;
using LogoFX.Client.Mvvm.ViewModel.Services;
using $saferootprojectname$.Client.Model.Contracts;

namespace $safeprojectname$.ViewModels
{
    public class WarehouseItemContainerViewModel : EditableObjectViewModel<IWarehouseItem>
    {
        private readonly IWarehouseItem _model;
        private readonly IViewModelCreatorService _viewModelCreatorService;
        private readonly IDataService _dataService;

        public WarehouseItemContainerViewModel(
            IWarehouseItem model,
            IViewModelCreatorService viewModelCreatorService,
            IDataService dataService) :
                base(model)
        {
            _model = model;
            _viewModelCreatorService = viewModelCreatorService;
            _dataService = dataService;
        }

        private WarehouseItemCommandsViewModel _commands;
        public WarehouseItemCommandsViewModel Commands => _commands ??
                                                          (_commands = _viewModelCreatorService.CreateViewModel<WarehouseItemCommandsViewModel>());

        private WarehouseItemViewModel _item;
        public WarehouseItemViewModel Item => _item ??
                                              (_item =
                                                  _viewModelCreatorService.CreateViewModel<IWarehouseItem, WarehouseItemViewModel>(_model));

        protected override async Task<bool> SaveMethod(IWarehouseItem model)
        {
            await _dataService.SaveWarehouseItemAsync(Model);
            return true;
        }
    }
}