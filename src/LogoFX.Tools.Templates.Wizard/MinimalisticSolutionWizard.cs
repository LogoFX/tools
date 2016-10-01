using LogoFX.Tools.Common;

namespace LogoFX.Tools.Templates.Wizard
{
    public sealed class MinimalisticSolutionWizard : SolutionWizard
    {
        protected override string GetTitle()
        {
        return "New Simple WPF Application";
        }

        protected override WizardConfigurationDto GetWizardConfiguration()
        {
            return null;
        }
    }
}
