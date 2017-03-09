using System.Windows.Input;
using Caliburn.Micro;
using LogoFX.Client.Mvvm.Commanding;
using LogoFX.Client.Mvvm.ViewModel.Shared;

namespace $safeprojectname$.ViewModels
{
    public class ExitOptionsViewModel : Screen
    {
        public ExitOptionsViewModel()
        {
            DisplayName = "Exit options";
        }

        public MessageResult Result { get; private set; }

        private ICommand _closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = ActionCommand<MessageResult>.Do(r =>
                {
                    Result = r;
                    TryClose(true);
                }));
            }
        }        
    }
}
