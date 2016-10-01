﻿using LogoFX.Tools.Common;

namespace LogoFX.Tools.Templates.Wizard
{
    public static class WizardConfigurationExtensions
    {
        public static bool ShowWizardWindow(this WizardConfigurationDto wizardConfiguration)
        {
            return wizardConfiguration.TestOption ||
                   wizardConfiguration.FakeOption ||
                   wizardConfiguration.Solutions?.Count > 0;
        }
    }
}