
namespace LogoFX.Tools.Templates.Wizard
{
    public sealed class MinimalisticSolutionWizard : SolutionWizard
    {
        protected override string GetTitle()
        {
        return "New Simple WPF Application";
        }

        protected override WizardConfiguration GetWizardConfiguration()
        {
            return null;
        }
    }
}
