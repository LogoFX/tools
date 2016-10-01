using LogoFX.Tools.Common;

namespace LogoFX.Tools.Templates.Wizard
{
    public sealed class UniversalSolutionWizard : SolutionWizard
    {
        protected override string GetTitle()
        {
            return "New LogoFX Universal Application";
        }

        protected override WizardConfigurationDto GetWizardConfiguration()
        {
            return null;
        }
    }
}
