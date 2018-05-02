using Caliburn.Micro;
using JetBrains.Annotations;
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

        public ShellViewModel(IDataService dataService, IViewModelCreatorService viewModelCreatorService)
        {
            _dataService = dataService;
            _viewModelCreatorService = viewModelCreatorService;
        }

        private async void StartAcivateMainViewModel()
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

        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set { Set(ref _isBusy, value); }
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            StartAcivateMainViewModel();
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