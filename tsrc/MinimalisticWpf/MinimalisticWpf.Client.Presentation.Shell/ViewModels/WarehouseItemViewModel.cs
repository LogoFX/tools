using JetBrains.Annotations;
using LogoFX.Client.Mvvm.ViewModel;
using MinimalisticWpf.Client.Model.Contracts;

namespace MinimalisticWpf.Client.Presentation.Shell.ViewModels
{
    [UsedImplicitly]
    public class WarehouseItemViewModel : ObjectViewModel<IWarehouseItem>
    {
        public WarehouseItemViewModel(IWarehouseItem model) : base(model)
        {

        }
    }
}