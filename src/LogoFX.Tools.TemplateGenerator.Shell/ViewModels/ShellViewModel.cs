using System.ComponentModel;
using System.Windows.Input;
using Caliburn.Micro;
using JetBrains.Annotations;
using LogoFX.Client.Mvvm.Commanding;
using LogoFX.Client.Mvvm.ViewModel.Contracts;
using LogoFX.Client.Mvvm.ViewModel.Services;
using LogoFX.Tools.TemplateGenerator.Model.Contract;
using LogoFX.Tools.TemplateGenerator.Shell.Properties;

namespace LogoFX.Tools.TemplateGenerator.Shell.ViewModels
{
    [UsedImplicitly]
    public sealed class ShellViewModel : Conductor<MainViewModel>, ICanBeBusy
    {
        private readonly IDataService _dataService;
        private readonly IViewModelCreatorService _viewModelCreatorService;
        private bool _closingState;

        public ShellViewModel(IDataService dataService, IViewModelCreatorService viewModelCreatorService)
        {
            _dataService = dataService;
            _viewModelCreatorService = viewModelCreatorService;
        }

        private ICommand _closingCommand;

        public ICommand ClosingCommand
        {
            get
            {
                return _closingCommand ??
                       (_closingCommand = ActionCommand<CancelEventArgs>
                           .Do(e =>
                           {
                               if (!_closingState)
                               {
                                   _closingState = true;
                                   e.Cancel = true;
                                   StartClose();
                               }
                           }));
            }
        }

        private async void StartClose()
        {
            await _dataService.SaveConfigurationAsync();
            TryClose();
        }

        private async void StartActivateMainViewModel()
        {
            IsBusy = true;

            try
            {
                var configuration = await _dataService.GetConfigurationAsync();
                ActivateItem(_viewModelCreatorService.CreateViewModel<IConfiguration, MainViewModel>(configuration));
            }

            finally
            {
                IsBusy = false;
            }
        }

        public override string DisplayName
        {
            get { return "LogoFX Template Generator"; }
            set { }
        }

        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set { Set(ref _isBusy, value); }
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            StartActivateMainViewModel();
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);

            if (close)
            {
                Settings.Default.Save();
            }
        }
    }
}