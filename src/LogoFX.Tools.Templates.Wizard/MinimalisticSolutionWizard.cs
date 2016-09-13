using LogoFX.Tools.TemplateGenerator;

namespace LogoFX.Tools.Templates.Wizard
{
    public sealed class MinimalisticSolutionWizard : SolutionWizard
    {
        protected override string GetTitle()
        {
            return "New LogoFX WPF Simple Application";
        }

        protected override WizardConfiguration GetWizardConfiguration()
        {
            return new WizardConfiguration();
        }
    }
}