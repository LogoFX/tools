using System;
using System.Windows.Input;
using Caliburn.Micro;
using JetBrains.Annotations;
using LogoFX.Client.Mvvm.Commanding;
using LogoFX.Client.Mvvm.ViewModel.Contracts;
using MinimalisticWpf.Client.Model.Contracts;

namespace MinimalisticWpf.Client.Presentation.Shell.ViewModels
{
    [UsedImplicitly]
    public sealed class LoginViewModel : Screen, ICanBeBusy
    {
        private readonly ILoginService _loginService;

        public LoginViewModel(ILoginService loginService)
        {
            _loginService = loginService;
        }

        private ICommand _loginCommand;

        public ICommand LoginCommand
        {
            get
            {
                return _loginCommand ??
                       (_loginCommand = ActionCommand
                           .When(() => !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password))
                           .Do(async () =>
                           {
                               LoginFailureCause = null;

                               try
                               {
                                   IsBusy = true;
                                   await _loginService.LoginAsync(UserName, Password);
                                   OnLoginSuccess();
                               }

                               catch (Exception err)
                               {
                                   LoginFailureCause = err.Message;
                               }

                               finally
                               {
                                   Password = string.Empty;
                                   IsBusy = false;
                               }
                           })
                           .RequeryOnPropertyChanged(this, () => UserName)
                           .RequeryOnPropertyChanged(this, () => Password));
            }
        }

        private ICommand _cancelCommand;

        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ??
                       (_cancelCommand = ActionCommand
                           .Do(() =>
                           {
                               TryClose(true);
                           }));
            }
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

        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set
            {
                if (_userName == value)
                {
                    return;
                }

                _userName = value;
                NotifyOfPropertyChange();
            }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password == value)
                {
                    return;
                }

                _password = value;
                NotifyOfPropertyChange();
            }
        }

        private string _loginFailureCause;

        public string LoginFailureCause
        {
            get { return _loginFailureCause; }
            private set
            {
                if (_loginFailureCause == value)
                {
                    return;
                }

                _loginFailureCause = value;
                NotifyOfPropertyChange();
            }
        }

        private void OnLoginSuccess()
        {
            TryClose(true);
        }
    }
}