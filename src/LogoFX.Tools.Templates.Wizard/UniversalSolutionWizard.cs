namespace LogoFX.Tools.Templates.Wizard
{
    public sealed class UniversalSolutionWizard : SolutionWizard
    {
        protected override string GetTitle()
        {
            return "New LogoFX Universal Application";
        }

        protected override WizardConfiguration GetWizardConfiguration()
        {
            return null;
        }
    }
}
