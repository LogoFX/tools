using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using JetBrains.Annotations;
using LogoFX.Tools.TemplateGenerator.Contracts;
using Microsoft.Expression.Interactivity.Core;

namespace LogoFX.Tools.TemplateGenerator.Shell.ViewModels
{
    [UsedImplicitly]
    public sealed class ShellViewModel : Screen
    {
        #region Fields

        private SolutionTemplateGenerator _solutionTemplateGenerator;
        private IConfiguration _configuration;
        private readonly IDataService _dataService;

        #endregion

        #region Constructors

        public ShellViewModel()
        {
            _dataService = IoC.Get<IDataService>();
        }

        #endregion

        #region Commands

        private ICommand _generateTemplateCommand;
        public ICommand GenerateTemplateCommand => _generateTemplateCommand ??
                                                   (_generateTemplateCommand = new ActionCommand(GenerateTemplate));

        private ICommand _saveCommand;
        public ICommand SaveCommand => _saveCommand ??
                                       (_saveCommand = new ActionCommand(SaveTemplate));

        private ICommand _makeMultiSolutionCommand;
        public ICommand MakeMultiSolutionCommand => _makeMultiSolutionCommand ??
                                                    (_makeMultiSolutionCommand = new ActionCommand(MakeMultiSolution));

        private ICommand _makeSingleSolutionCommand;
        public ICommand MakeSingleSolutionCommand => _makeSingleSolutionCommand ??
                                                    (_makeSingleSolutionCommand = new ActionCommand(MakeSingleSolution));

        #endregion

        #region Public Properties

        public bool IsMultisolution
        {
            get
            {
                if (ActiveConfiguration == null)
                {
                    return false;
                }

                return ActiveConfiguration.IsMultisolution;
            }
        }

        private SolutionConfigurationViewModel _activeConfiguration;

        public SolutionConfigurationViewModel ActiveConfiguration
        {
            get { return _activeConfiguration; }
            private set
            {
                if (_activeConfiguration == value)
                {
                    return;
                }

                if (_activeConfiguration != null)
                {
                    _activeConfiguration.CanGenerateUpdated -= OnCanGenerateUpdated;
                }

                _activeConfiguration = value;

                if (_activeConfiguration != null)
                {
                    _activeConfiguration.CanGenerateUpdated += OnCanGenerateUpdated;
                }

                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => CanGenerate);
            }
        }

        private void OnCanGenerateUpdated(object sender, EventArgs e)
        {
            NotifyOfPropertyChange(() => CanGenerate);
            NotifyOfPropertyChange(() => IsMultisolution);
        }

        private ISolutionTemplateInfo _solutionTemplateInfo;

        public ISolutionTemplateInfo SolutionTemplateInfo
        {
            get { return _solutionTemplateInfo; }
            private set
            {
                if (_solutionTemplateInfo == value)
                {
                    return;
                }

                _solutionTemplateInfo = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => CanGenerate);
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

        public bool CanGenerate
        {
            get
            {
                if (!ActiveConfiguration.CanGenerate ||
                   SolutionTemplateInfo == null)
                {
                    return false;
                }

                return true;
            }
        }

        #endregion

        #region Private Members

        private void SaveTemplate()
        {
            Save(true);
        }

        private Task CleanDestinationFolderAsync()
        {
            var destinationPath = ActiveConfiguration.DestinationPath;

            return Task.Run(() =>
            {
                if (Directory.Exists(destinationPath))
                {
                    Directory.Delete(destinationPath, true);
                }
            });
        }

        private async void MakeMultiSolution()
        {
            Debug.Assert(!IsMultisolution);

            IsBusy = true;

            try
            {
                await MakeMultiSolutionAsync();
                await GetInfoAsync();
            }

            finally
            {
                IsBusy = false;
            }
        }

        private async Task MakeMultiSolutionAsync()
        {
            await CleanDestinationFolderAsync();

            await ActiveConfiguration.MakeMultiSolutionAsync();

            NotifyOfPropertyChange(() => IsMultisolution);
        }

        private async void MakeSingleSolution()
        {
            Debug.Assert(IsMultisolution);

            IsBusy = true;

            try
            {
                await MakeSingleSolutionAsync();
                await GetInfoAsync();
            }

            finally
            {
                IsBusy = false;
            }
        }

        private async Task MakeSingleSolutionAsync()
        {
            await CleanDestinationFolderAsync();

            ActiveConfiguration.MakeSingleSolution();

            NotifyOfPropertyChange(() => IsMultisolution);
        }

        private async void GenerateTemplate()
        {
            IsBusy = true;

            try
            {
                await _solutionTemplateGenerator.GenerateAsync(
                    ActiveConfiguration.DestinationPath,
                    SolutionTemplateInfo,
                    ActiveConfiguration.WizardConfiguration.Model);

                await SaveAsync(true);

                SolutionTemplateInfo = null;

                TryClose();
            }

            finally
            {
                IsBusy = false;
            }
        }

        private async Task GetInfoAsync(string solutionFileName)
        {
            _configuration = _dataService.LoadConfiguration();
            var solutionConfiguration = _configuration.SolutionConfigurations
                .SingleOrDefault(x => x.FileName == solutionFileName);
            if (solutionConfiguration == null)
            {
                solutionConfiguration = _configuration.CreateNewSolutionConfiguration(solutionFileName);
                await SaveAsync(false);
            }
            ActiveConfiguration = new SolutionConfigurationViewModel(solutionConfiguration, async wc =>
            {
                _solutionTemplateGenerator = new SolutionTemplateGenerator(solutionFileName, IsMultisolution);
                SolutionTemplateInfo = await _solutionTemplateGenerator.GetInfoAsync();
                ActiveConfiguration.AddCurrentSolutionConfiguration();
            });
        }

        private async Task SaveAsync(bool saveWizardConfiguration)
        {
            if (_configuration != null)
            {
                _dataService.SaveConfiguration(_configuration);
            }

            if (saveWizardConfiguration && ActiveConfiguration != null)
            {
                await ActiveConfiguration.SaveWizardConfigurationAsync();
            }
        }

        private async void Save(bool saveWizardConfiguration)
        {
            IsBusy = true;

            try
            {
                await SaveAsync(saveWizardConfiguration);
            }

            finally
            {
                IsBusy = false;
            }
        }

        #endregion

        #region Overrides

        public override string DisplayName
        {
            get { return "Template Generator"; }
            set { }
        }

        private async Task GetInfoAsync()
        {
            var solutionFileName = _dataService.GetSolutionFileName();

            if (string.IsNullOrEmpty(solutionFileName))
            {
                MessageBox.Show(Application.Current.MainWindow,
                    "Solution not loaded.",
                    DisplayName,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                await Task.Run(() =>
                {
                    TryClose();
                });

                return;
            }

            await GetInfoAsync(solutionFileName);
        }

        protected override async void OnActivate()
        {
            base.OnActivate();

            IsBusy = true;
            try
            {
                await GetInfoAsync();
            }

            finally
            {
                IsBusy = false;
            }
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);

            if (close)
            {
                Save(false);
            }
        }

        #endregion
    }
}