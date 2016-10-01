using System.Collections.Generic;
using LogoFX.Tools.TemplateGenerator;

namespace LogoFX.Tools.Templates.Wizard
{
    public sealed class WizardSolutionWizard : SolutionWizard
    {
        protected override string GetTitle()
        {
            return "New LogoFX Application";
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
                        Name = "Specifications",
                        Caption = "Specifications",
                        IconName = "",
                    },
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
