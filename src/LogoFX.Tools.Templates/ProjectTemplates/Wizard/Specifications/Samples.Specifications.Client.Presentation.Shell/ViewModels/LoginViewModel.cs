using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using JetBrains.Annotations;
using LogoFX.Client.Mvvm.Commanding;
using LogoFX.Client.Mvvm.ViewModel.Extensions;
using $saferootprojectname$.Client.Model.Contracts;
using $safeprojectname$.Properties;

namespace $safeprojectname$.ViewModels
{   
    [UsedImplicitly]
    public sealed class LoginViewModel : BusyScreen
    {
        private readonly ILoginService _loginService;

        public LoginViewModel(
            ILoginService loginService)
        {
            SavePassword = Settings.Default.SavePassword;
            UserName = Settings.Default.SavedUsername;
            if (SavePassword)
            {
                Password = Settings.Default.SavedPassword;
            }
            else
            {
                Password = string.Empty;
            }

            _loginService = loginService;
            DisplayName = "Login View";
        }

        public event EventHandler LoggedInSuccessfully;

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

                               catch (Exception ex)
                               {
                                   LoginFailureCause = "Failed to log in: " + ex.Message;
                               }

                               finally
                               {
                                   Password = string.Empty;
                                   IsBusy = false;
                               }
                           }));
            }
        }

        private ICommand _cancelCommand;

        public ICommand LoginCancelCommand
        {
            get
            {
                return _cancelCommand ??
                       (_cancelCommand = ActionCommand
                           .Do(() =>
                           {
                               TryClose();
                           }));
            }
        }

        private bool _savePassword;

        public bool SavePassword
        {
            get { return _savePassword; }
            set
            {
                if (_savePassword == value)
                {
                    return;
                }

                _savePassword = value;
                NotifyOfPropertyChange();
            }
        }

        private string _loginFailureCause;

        public string LoginFailureCause
        {
            get { return _loginFailureCause; }
            set
            {
                if (_loginFailureCause == value)
                    return;

                _loginFailureCause = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => IsLoginFailureTextVisible);
            }
        }

        public bool IsLoginFailureTextVisible
        {
            get { return string.IsNullOrWhiteSpace(LoginFailureCause) == false; }
        }
        
        private bool _isUserAuthenticated;

        public bool IsUserAuthenticated
        {
            get { return _isUserAuthenticated; }
            private set
            {
                if (_isUserAuthenticated == value)
                    return;

                _isUserAuthenticated = value;
                NotifyOfPropertyChange();
            }
        }              

        public string CurrentDomain
        {
            get;
            private set;
        }

        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                NotifyOfPropertyChange();
            }
        }
       
        private string _password = string.Empty;

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password == value)
                    return;

                _password = value;
                NotifyOfPropertyChange();
            }
        }        
                
        private void OnLoginSuccess()
        {
            Settings.Default.SavePassword = SavePassword;
            Settings.Default.SavedUsername = UserName;

            if (SavePassword)
            {
                Settings.Default.SavedPassword = Password;
            }
            else
            {
                Settings.Default.SavedPassword = string.Empty;
            }

            TryClose(true);

            if (LoggedInSuccessfully != null)
            {
                LoggedInSuccessfully(this, new EventArgs());
            }
        }        
    }
}
