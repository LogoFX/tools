using Caliburn.Micro;
using LogoFX.Client.Mvvm.ViewModel;
using $saferootprojectname$.Client.Model.Contracts;

namespace $safeprojectname$.ViewModels
{    
    public class WarehouseItemViewModel : ObjectViewModel<IWarehouseItem>
    {
        public WarehouseItemViewModel(
            IWarehouseItem model) : base(model)
        {
        }

        
    }

    public class WarehouseItemCommandsViewModel : PropertyChangedBase
    {
        
    }
}
