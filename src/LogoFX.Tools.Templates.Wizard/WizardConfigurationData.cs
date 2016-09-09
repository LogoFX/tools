using System.Collections.Generic;
using LogoFX.Tools.TemplateGenerator;

namespace LogoFX.Tools.Templates.Wizard
{
    internal static class WizardConfigurationData
    {
        public static WizardConfiguration GetWizardConfiguration()
        {
            return new WizardConfiguration
            {
                FakeOption = true,
                TestOption = true,
                Solutions = new List<SolutionInfo>
                {
                    new SolutionInfo
                    {
                        Name = "Specifications",
                        Caption = "WPF Desktop Application",
                        IconName = ""
                    }
                }
            };
        }
    }
}