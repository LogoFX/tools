using LogoFX.Client.Mvvm.ViewModel.Extensions;
using System.Threading.Tasks;

namespace $rootnamespace$
{
    public class $safeitemrootname$ViewModel : EditableObjectViewModel<I$safeitemrootname$>
    {
        public EditableViewModel(I$safeitemrootname$ model)
            : base(model)
        {
            IsEnabled = model != null;
        }

        protected override Task<bool> SaveMethod(I$safeitemrootname$ domainModel)
        {
            return TaskRunner.RunAsync(() => true);
        }
    }
}