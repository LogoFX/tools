using System.Collections.Generic;
using LogoFX.Tools.TemplateGenerator;

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
            return new WizardConfiguration
            {
                FakeOption=false,
                TestOption=false,
            };
        }
    }
}
