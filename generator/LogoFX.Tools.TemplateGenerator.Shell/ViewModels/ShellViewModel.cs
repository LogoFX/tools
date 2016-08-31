using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Avalon.Windows.Dialogs;
using Caliburn.Micro;
using EnvDTE;
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

        #endregion

        #region Private Members

        private async Task GetInfoAsync(Solution solution)
        {
            var solutionFileName = solution.FullName;
            _configuration = _dataService.LoadConfiguration();
            var solutionConfiguration = _configuration.SolutionConfigurations
                .SingleOrDefault(x => x.FileName == solutionFileName);
            if (solutionConfiguration == null)
            {
                solutionConfiguration = _configuration.CreateNewSolutionConfiguration(solutionFileName);
                _dataService.SaveConfiguration(_configuration);
            }
            ActiveConfiguration = new SolutionConfigurationViewModel(solutionConfiguration);

            _solutionTemplateGenerator = new SolutionTemplateGenerator(solution);
            SolutionTemplateInfo = await _solutionTemplateGenerator.GetInfoAsync();
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

            var dte = _dataService.GetDte();
            var solution = dte.Solution;

            if (solution == null || string.IsNullOrEmpty(solution.FullName))
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
                await GetInfoAsync(solution);
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