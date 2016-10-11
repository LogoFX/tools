using JetBrains.Annotations;
using LogoFX.Client.Mvvm.ViewModel;
using $saferootprojectname$.Client.Model.Contracts;

namespace $safeprojectname$.ViewModels
{
    [UsedImplicitly]
    public class WarehouseItemViewModel : ObjectViewModel<IWarehouseItem>
    {
        public WarehouseItemViewModel(IWarehouseItem model) : base(model)
        {
            
        }

        public string Kind
        {
            get { return Model.Kind; }
        }

        public int Quantity
        {
            get { return Model.Quantity; }
        }

        public double Price
        {
            get { return Model.Price; }
        }

        public double TotalCost
        {
            get { return Model.TotalCost; }
        }
    }
}
