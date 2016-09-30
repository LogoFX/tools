using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalon.Windows.Dialogs;
using Caliburn.Micro;
using JetBrains.Annotations;
using LogoFX.Client.Mvvm.Commanding;
using LogoFX.Tools.TemplateGenerator.Contracts;
using LogoFX.Tools.TemplateGenerator.Shell.Properties;

namespace LogoFX.Tools.TemplateGenerator.Shell.ViewModels
{
    [UsedImplicitly]
    public sealed partial class ShellViewModel : Screen
    {
        #region Fields

        private SolutionTemplateGenerator _solutionTemplateGenerator;
        private readonly IDataService _dataService;

        #endregion

        #region Constructors

        public ShellViewModel(IDataService dataService)
        {
            _dataService = dataService;

            DestinationPath = Settings.Default.DestinationPath;
        }

        #endregion

        #region Commands

        private ICommand _generateTemplateCommand;

        public ICommand GenerateTemplateCommand
        {
            get
            {
                return _generateTemplateCommand ??
                       (_generateTemplateCommand = ActionCommand
                           .When(() => CanGenerate)
                           .Do(GenerateTemplate)
                           .RequeryOnPropertyChanged(this, () => CanGenerate));
            }
        }

        private ICommand _saveCommand;

        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand ??
                       (_saveCommand = ActionCommand
                           .When(() => !string.IsNullOrEmpty(DestinationPath))
                           .Do(SaveTemplate)
                           .RequeryOnPropertyChanged(this, () => DestinationPath));
            }
        }

        private ICommand _makeMultiSolutionCommand;

        public ICommand MakeMultiSolutionCommand
        {
            get
            {
                return _makeMultiSolutionCommand ??
                       (_makeMultiSolutionCommand = ActionCommand
                           .When(() => !IsMultisolution && !string.IsNullOrEmpty(DestinationPath))
                           .Do(MakeMultiSolution)
                           .RequeryOnPropertyChanged(this, () => IsMultisolution)
                           .RequeryOnPropertyChanged(this, () => DestinationPath));
            }
        }

        private ICommand _makeSingleSolutionCommand;

        public ICommand MakeSingleSolutionCommand
        {
            get
            {
                return _makeSingleSolutionCommand ??
                       (_makeSingleSolutionCommand = ActionCommand
                           .When(() => IsMultisolution && !string.IsNullOrEmpty(DestinationPath))
                           .Do(MakeSingleSolution)
                           .RequeryOnPropertyChanged(this, () => IsMultisolution)
                           .RequeryOnPropertyChanged(this, () => DestinationPath));
            }
        }

        private ICommand _browseDestinationFolderCommand;
        public ICommand BrowseDestinationFolderCommand
        {
            get
            {
                return _browseDestinationFolderCommand ??
                       (_browseDestinationFolderCommand = ActionCommand
                           .When(() => true)
                           .Do(() =>
                           {
                               FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
                               {
                                   RootSpecialFolder = Environment.SpecialFolder.MyComputer,
                                   SelectedPath = DestinationPath,
                                   Title = "Destination Path"
                               };

                               var retVal = folderBrowserDialog.ShowDialog() ?? false;

                               if (!retVal)
                               {
                                   return;
                               }

                               DestinationPath = folderBrowserDialog.SelectedPath;
                               Settings.Default.DestinationPath = DestinationPath;
                               Settings.Default.Save();
                           }));
            }
        }

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

        private void OnCanGenerateUpdated()
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
                if (WizardConfiguration == null ||
                    !WizardConfiguration.CanGenerate ||
                    SolutionTemplateInfo == null)
                {
                    return false;
                }

                return true;
            }
        }

        private string _destinationPath;
        public string DestinationPath
        {
            get { return _destinationPath; }
            private set
            {
                if (value == _destinationPath)
                {
                    return;
                }

                _destinationPath = value;
                NotifyOfPropertyChange();
                UpdateWizardConfiguration();
            }
        }

        #endregion

        #region Private Members

        private void SaveTemplate()
        {
            Save();
        }

        private Task CleanDestinationFolderAsync()
        {
            var destinationPath = DestinationPath;

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

            WizardConfiguration.Model.IsMultisolution = true;

            await SaveWizardConfigurationAsync();

            NotifyOfPropertyChange(() => IsMultisolution);
        }

        public async Task SaveWizardConfigurationAsync()
        {
            var destinationPath = DestinationPath;

            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            WizardConfiguration wizardConfiguration = WizardConfiguration.Model;
            var fileName = WizardConfigurator.GetWizardConfigurationFileName(destinationPath);
            await WizardConfigurator.SaveAsync(fileName, wizardConfiguration);
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

            foreach (var solution in WizardConfiguration.Model.Solutions.ToList())
            {
                WizardConfiguration.Model.Solutions.Remove(solution);
            }

            WizardConfiguration.Model.IsMultisolution = false;

            await SaveWizardConfigurationAsync();

            NotifyOfPropertyChange(() => IsMultisolution);
        }

        private async void GenerateTemplate()
        {
            IsBusy = true;

            try
            {
                await _solutionTemplateGenerator.GenerateAsync(
                    DestinationPath,
                    SolutionTemplateInfo,
                    WizardConfiguration.Model);

                await SaveAsync();

                SolutionTemplateInfo = null;

                TryClose();
            }

            finally
            {
                IsBusy = false;
            }
        }

        private async Task SaveAsync()
        {
            if (WizardConfiguration == null)
            {
                return;
            }

            await SaveWizardConfigurationAsync();
        }

        private async void Save()
        {
            IsBusy = true;

            try
            {
                await SaveAsync();
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

            IsBusy = true;
            try
            {
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
                Save();
            }
        }

        #endregion
    }
}