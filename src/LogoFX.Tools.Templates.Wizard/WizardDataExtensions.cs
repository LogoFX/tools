using LogoFX.Tools.Common.Model;

namespace LogoFX.Tools.Templates.Wizard
{
    public static class WizardDataExtensions
    {
        public static bool ShowWizardWindow(this WizardData wizardData)
        {
            return true;

            //return wizardConfiguration.TestOption ||
            //       wizardConfiguration.FakeOption ||
            //       wizardConfiguration.Solutions.Any();
        }
    }
}