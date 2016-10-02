using System.Linq;

namespace LogoFX.Tools.Templates.Wizard
{
    public static class WizardConfigurationExtensions
    {
        public static bool ShowWizardWindow(this WizardConfiguration wizardConfiguration)
        {
            return wizardConfiguration.TestOption ||
                   wizardConfiguration.FakeOption ||
                   wizardConfiguration.Solutions.Any();
        }
    }
}