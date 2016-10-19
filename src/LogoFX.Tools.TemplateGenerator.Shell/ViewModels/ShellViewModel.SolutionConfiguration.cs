using System;
using System.IO;
using System.Threading.Tasks;

namespace LogoFX.Tools.TemplateGenerator.Shell.ViewModels
{
    public sealed partial class ShellViewModel
    {
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
                WizardConfiguration = new WizardConfigurationViewModel(wizardConfiguration, _windowManager);
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
                await Task.Delay(100);
            }
            else
            {
                wizardConfiguration = await WizardConfigurator.LoadAsync(fileName);
            }

            return wizardConfiguration;
        }
    }
}