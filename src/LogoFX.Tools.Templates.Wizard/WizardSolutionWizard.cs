using System.Collections.Generic;

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
                        Caption = "LogoFX Desktop Application",
                        IconName = "",
                    },
                    new SolutionInfo
                    {
                        Name = "Samples.Universal",
                        Caption = "LogoFX UWP Application",
                        IconName = "",
                    },
                },
            };
        }
    }
}
