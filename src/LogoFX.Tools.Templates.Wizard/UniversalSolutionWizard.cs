using System.Collections.Generic;
using LogoFX.Tools.TemplateGenerator;

namespace LogoFX.Tools.Templates.Wizard
{
    public sealed class UniversalSolutionWizard : SolutionWizard
    {
        protected override string GetTitle()
        {
            return "New LogoFX UWP Application";
        }

        protected override WizardConfiguration GetWizardConfiguration()
        {
            return new WizardConfiguration
            {
                FakeOption=true,
                TestOption=true,
                Solutions = new List<SolutionInfo>
                {
                    new SolutionInfo
                    {
                        Name = "Samples.Universal",
                        Caption = "Samples.Universal",
                        IconName = "",
                    },
                },
            };
        }
    }
}
