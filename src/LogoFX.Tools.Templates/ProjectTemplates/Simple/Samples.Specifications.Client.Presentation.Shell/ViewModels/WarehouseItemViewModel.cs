using JetBrains.Annotations;
using LogoFX.Client.Mvvm.ViewModel;
using $saferootprojectname$.Client.Model.Contracts;

namespace $safeprojectname$.ViewModels
{
    [UsedImplicitly]
    public class WarehouseItemViewModel : /*Editable*/ObjectViewModel<IWarehouseItem>
    {
        public WarehouseItemViewModel(IWarehouseItem model) 
            : base(model)
        {
            IsEnabled = model != null;
        }

        //public bool IsEnabled
        //{
        //    get { return Model != null; }
        //}

        //protected override Task<bool> SaveMethod(IWarehouseItem model)
        //{
        //    return TaskRunner.RunAsync(() => true);
        //}
    }
}
