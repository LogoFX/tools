using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalon.Windows.Dialogs;
using LogoFX.Tools.TemplateGenerator.Contracts;
using Microsoft.Expression.Interactivity.Core;

namespace LogoFX.Tools.TemplateGenerator.Shell.ViewModels
{
    public sealed class SolutionConfigurationViewModel : CanGenerateViewModel<ISolutionConfiguration>
    {
        #region Fields

        public SolutionConfigurationViewModel(ISolutionConfiguration model) 
            : base(model)
        {
            DestinationPath = model.DestinationPath;
        }

        #endregion

        #region Commands

        private ICommand _browseDestinationFolderCommand;

        public ICommand BrowseDestinationFolderCommand
        {
            get
            {
                return _browseDestinationFolderCommand ??
                       (_browseDestinationFolderCommand = new ActionCommand(() =>
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
                       }));
            }
        }

        #endregion

        #region Public Properties

        public string SolutionFileName => Model.FileName;

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
                Model.DestinationPath = value;
                NotifyOfPropertyChange();
                UpdateWizardConfiguration();
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

                if (_wizardConfiguration != null)
                {
                    _wizardConfiguration.CanGenerateUpdated -= CanGenerateChanged;
                }

                _wizardConfiguration = value;

                if (_wizardConfiguration != null)
                {
                    _wizardConfiguration.CanGenerateUpdated += CanGenerateChanged;
                }

                NotifyOfPropertyChange();

                OnCanGenerateUpdated();
            }
        }

        private void CanGenerateChanged(object sender, EventArgs e)
        {
            OnCanGenerateUpdated();
        }

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

        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            private set
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

        #region Public Methods

        public async Task MakeMultiSolutionAsync()
        {
            var name = Path.GetFileNameWithoutExtension(SolutionFileName);

            WizardConfiguration.Model.Solutions.Add(new SolutionInfo
            {
                Caption = name,
                IconName = string.Empty,
                Name = name
            });

            await SaveWizardConfigurationAsync();

            NotifyOfPropertyChange(() => IsMultisolution);
        }

        public void MakeSingleSolution()
        {
            WizardConfiguration.Model.Solutions.Clear();

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

        #endregion

        #region Private Members

        private async void UpdateWizardConfiguration()
        {
            if (string.IsNullOrEmpty(DestinationPath))
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

        private async Task<WizardConfiguration> UpdateWizardConfigurationAsync()
        {
            WizardConfiguration wizardConfiguration;
            var fileName = WizardConfigurator.GetWizardConfigurationFileName(DestinationPath);

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

        #endregion

        #region Overrides

        protected override bool GetCanGenerate()
        {
            //return !string.IsNullOrWhiteSpace(DestinationPath);
            return WizardConfiguration != null && WizardConfiguration.CanGenerate;
        }

        #endregion
    }
}