using System.Windows.Input;
using Caliburn.Micro;
using JetBrains.Annotations;
using LogoFX.Client.Mvvm.Commanding;
using LogoFX.Client.Mvvm.ViewModel.Contracts;
using LogoFX.Client.Mvvm.ViewModel.Services;
using MinimalisticWpf.Client.Model.Shared;

namespace MinimalisticWpf.Client.Presentation.Shell.ViewModels
{
    [UsedImplicitly]
    public sealed class ShellViewModel : Conductor<IScreen>, ICanBeBusy
    {
        private readonly IViewModelCreatorService _viewModelCreatorService;

        public ShellViewModel(IViewModelCreatorService viewModelCreatorService)
        {
            _viewModelCreatorService = viewModelCreatorService;
        }

        private ICommand _closeCommand;

        public ICommand CloseCommand
        {
            get
            {
                return _closeCommand ??
                       (_closeCommand = ActionCommand
                           .Do(() =>
                           {
                               TryClose();
                           }));
            }
        }

        public bool IsLoggedIn
        {
            get { return UserContext.Current != null; }
        }

        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy == value)
                {
                    return;
                }

                _isBusy = value;
                NotifyOfPropertyChange();
            }
        }

        public override string DisplayName
        {
            get { return "Minimalistic WPF LogoFX Sample Application"; }
            set { }
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            ShowLoginOrMainView();
        }

        public override void DeactivateItem(IScreen item, bool close)
        {
            base.DeactivateItem(item, close);

            if (close && item is LoginViewModel)
            {
                ShowLoginOrMainView();
            }
        }

        private void ShowLoginOrMainView()
        {
            IScreen subViewModel;

            if (IsLoggedIn)
            {
                subViewModel = _viewModelCreatorService.CreateViewModel<MainViewModel>();
            }
            else
            {
                subViewModel = _viewModelCreatorService.CreateViewModel<LoginViewModel>();
            }

            ActivateItem(subViewModel);
        }
    }
}