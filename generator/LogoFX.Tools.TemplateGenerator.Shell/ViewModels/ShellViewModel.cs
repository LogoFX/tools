using System;
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

        public ICommand GenerateTemplateCommand
        {
            get
            {
                return _generateTemplateCommand ??
                       (_generateTemplateCommand = new ActionCommand(async () =>
                       {
                           if (!ActiveConfiguration.CanGenerate ||
                               SolutionTemplateInfo == null)
                           {
                               return;
                           }

                           IsBusy = true;

                           try
                           {
                               TemplateDataInfo templateData = new TemplateDataInfo
                               {
                                   DefaultName = ActiveConfiguration.DefaultName,
                                   Description = ActiveConfiguration.Description,
                                   Name = ActiveConfiguration.Name
                               };

                               await _solutionTemplateGenerator.GenerateAsync(templateData, ActiveConfiguration.DestinationPath, SolutionTemplateInfo);
                               SolutionTemplateInfo = null;

                               TryClose();
                           }

                           finally
                           {
                               IsBusy = false;
                           }
                       }));
            }
        }

        #endregion

        #region Public Properties

        private bool _isMultisolution;

        public bool IsMultisolution
        {
            get { return _isMultisolution; }
            set
            {
                if (_isMultisolution == value)
                {
                    return;
                }

                _isMultisolution = value;
                NotifyOfPropertyChange();

                UpdateMultisolutionInfo();
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

                _activeConfiguration = value;
                NotifyOfPropertyChange();
            }
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
            }
        }

        private bool _isBusy;
        private IConfiguration _configuration;

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
            }
        }

        #endregion

        #region Private Members

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

        private async Task<WizardConfiguration> UpdateMultisolutionInfoAsync()
        {
            var wizardConfigurator = new WizardConfigurator();
            var fileName = wizardConfigurator.GetWizardConfigurationFileName(ActiveConfiguration.DestinationPath);

            if (File.Exists(fileName))
            {
                return null;
            }

            var wizardConfiguration = await wizardConfigurator.LoadAsync(fileName);
            return wizardConfiguration;
        }

        private async void UpdateMultisolutionInfo()
        {
            if (!IsMultisolution)
            {
                WizardConfiguration = null;
            }

            IsBusy = true;

            try
            {
                var wizardConfiguration = await UpdateMultisolutionInfoAsync();
                WizardConfiguration = wizardConfiguration == null
                    ? null
                    : new WizardConfigurationViewModel(wizardConfiguration);
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