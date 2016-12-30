using System.Threading.Tasks;
using JetBrains.Annotations;
using LogoFX.Client.Mvvm.ViewModel.Extensions;
using $saferootprojectname$.Client.Model.Contracts;
using Solid.Practices.Scheduling;

namespace $safeprojectname$.ViewModels
{
    [UsedImplicitly]
    public class WarehouseItemViewModel : EditableObjectViewModel<IWarehouseItem>
    {
        public WarehouseItemViewModel(IWarehouseItem model) 
            : base(model)
        {
            IsEnabled = model != null;
        }

        private bool _isNew;

        public bool IsNew
        {
            get { return _isNew; }
            set
            {
                if (_isNew == value)
                {
                    return;
                }

                _isNew = value;
                NotifyOfPropertyChange();
            }
        }

        protected override Task<bool> SaveMethod(IWarehouseItem model)
        {
            return TaskRunner.RunAsync(() => true);
        }
    }
}
