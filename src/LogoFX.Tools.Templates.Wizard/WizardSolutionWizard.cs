using System.Collections.Generic;
using LogoFX.Tools.Common;

namespace LogoFX.Tools.Templates.Wizard
{
    public sealed class WizardSolutionWizard : SolutionWizard
    {
        protected override string GetTitle()
        {
            return "New LogoFX Application";
        }

        protected override WizardConfigurationDto GetWizardConfiguration()
        {
            return new WizardConfigurationDto
            {
                FakeOption=true,
                TestOption=true,
                Solutions = new List<SolutionInfoDto>
                {
                    new SolutionInfoDto
                    {
                        Name = "Specifications",
                        Caption = "Specifications",
                        IconName = "",
                    },
                },
            };
        }
    }
}
