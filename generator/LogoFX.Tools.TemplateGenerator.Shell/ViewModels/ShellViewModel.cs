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
                if (WizardConfiguration == null)
                {
                    return false;
                }

                return WizardConfiguration.IsMultisolution;
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
                    _activeConfiguration.DestinationPathChanged -= OnDestinationPathChanged;
                    _activeConfiguration.CanGenerateUpdated -= OnCanGenerateUpdated;
                }

                _activeConfiguration = value;

                if (_activeConfiguration != null)
                {
                    _activeConfiguration.CanGenerateUpdated += OnCanGenerateUpdated;
                    _activeConfiguration.DestinationPathChanged += OnDestinationPathChanged;
                }

                NotifyOfPropertyChange();
            }
        }

        private void OnCanGenerateUpdated(object sender, EventArgs e)
        {
            NotifyOfPropertyChange(() => CanGenerate);
        }

        private void OnDestinationPathChanged(object sender, EventArgs e)
        {
            UpdateWizardConfiguration();
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

        private WizardConfigurationViewModel _wizardConfiguration;

        public WizardConfigurationViewModel WizardConfiguration
        {
            get { return _wizardConfiguration; }
            private set
            {
                if (_wizardConfiguration == value)
                {
                    return;
                }

                _wizardConfiguration = value;
                NotifyOfPropertyChange();

                NotifyOfPropertyChange(() => CanGenerate);
                NotifyOfPropertyChange(() => IsMultisolution);
            }
        }

        public bool CanGenerate
        {
            get
            {
                if (!ActiveConfiguration.CanGenerate ||
                   SolutionTemplateInfo == null ||
                   WizardConfiguration == null)
                {
                    return false;
                }

                return true;
            }
        }

        #endregion

        #region Private Members

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
            }

            finally
            {
                IsBusy = false;
            }
        }

        private async Task MakeMultiSolutionAsync()
        {
            await CleanDestinationFolderAsync();

            var name = Path.GetFileNameWithoutExtension(ActiveConfiguration.SolutionFileName);

            WizardConfiguration.Model.Solutions.Add(new SolutionInfo
            {
                Caption = name,
                IconName = string.Empty,
                Name = name
            });

            await SaveWizardConfigurationAsync();

            NotifyOfPropertyChange(() => IsMultisolution);
        }

        private async void MakeSingleSolution()
        {
            Debug.Assert(IsMultisolution);

            IsBusy = true;

            try
            {
                await MakeSingleSolutionAsync();
            }

            finally
            {
                IsBusy = false;
            }
        }

        private async Task MakeSingleSolutionAsync()
        {
            await CleanDestinationFolderAsync();

            WizardConfiguration.Model.Solutions.Clear();

            NotifyOfPropertyChange(() => IsMultisolution);
        }

        private async void GenerateTemplate()
        {
            IsBusy = true;

            try
            {
                TemplateDataInfo templateData = new TemplateDataInfo
                {
                    DefaultName = ActiveConfiguration.DefaultName,
                    Description = ActiveConfiguration.Description,
                    Name = ActiveConfiguration.Name
                };

                WizardConfiguration wizardConfiguration = null;

                if (IsMultisolution)
                {
                    await SaveWizardConfigurationAsync();
                    wizardConfiguration = WizardConfiguration.Model;
                }

                await _solutionTemplateGenerator.GenerateAsync(
                    templateData,
                    ActiveConfiguration.DestinationPath,
                    SolutionTemplateInfo,
                    wizardConfiguration);

                SolutionTemplateInfo = null;

                TryClose();
            }

            finally
            {
                IsBusy = false;
            }
        }

        private async Task SaveWizardConfigurationAsync()
        {
            var destinationPath = ActiveConfiguration.DestinationPath;

            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            WizardConfiguration wizardConfiguration = WizardConfiguration.Model;
            var fileName = WizardConfigurator.GetWizardConfigurationFileName(destinationPath);
            await WizardConfigurator.SaveAsync(fileName, wizardConfiguration);
        }

        private async Task GetInfoAsync(string solutionFileName)
        {
            _configuration = _dataService.LoadConfiguration();
            var solutionConfiguration = _configuration.SolutionConfigurations
                .SingleOrDefault(x => x.FileName == solutionFileName);
            if (solutionConfiguration == null)
            {
                solutionConfiguration = _configuration.CreateNewSolutionConfiguration(solutionFileName);
                _dataService.SaveConfiguration(_configuration);
            }
            ActiveConfiguration = new SolutionConfigurationViewModel(solutionConfiguration);

            _solutionTemplateGenerator = new SolutionTemplateGenerator(solutionFileName);
            SolutionTemplateInfo = await _solutionTemplateGenerator.GetInfoAsync();
        }

        private async Task<WizardConfiguration> UpdateWizardConfigurationAsync()
        {
            WizardConfiguration wizardConfiguration;
            var fileName = WizardConfigurator.GetWizardConfigurationFileName(ActiveConfiguration.DestinationPath);

            if (!File.Exists(fileName))
            {
                wizardConfiguration = new WizardConfiguration();
            }
            else
            {
                wizardConfiguration = await WizardConfigurator.LoadAsync(fileName);
            }

            return wizardConfiguration;
        }

        private async void UpdateWizardConfiguration()
        {
            if (string.IsNullOrEmpty(ActiveConfiguration.DestinationPath))
            {
                WizardConfiguration = null;
                return;
            }

            IsBusy = true;

            try
            {
                var wizardConfiguration = await UpdateWizardConfigurationAsync();
                WizardConfiguration = new WizardConfigurationViewModel(wizardConfiguration);
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

        protected override async void OnActivate()
        {
            base.OnActivate();

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

            IsBusy = true;
            try
            {
                await GetInfoAsync(solutionFileName);
            }

            finally
            {
                IsBusy = false;
            }

            UpdateWizardConfiguration();
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);

            if (close)
            {
                if (_configuration != null)
                {
                    _dataService.SaveConfiguration(_configuration);
                }
            }
        }

        #endregion
    }
}