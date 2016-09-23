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
                        Caption = "WPF Desktop Application",
                        IconName = "",
                        Items = new List<SolutionItemInfo>
                        {
                            new SolutionFolderInfo
                            {
                                Name = "App",
                                Items = new List<SolutionItemInfo>
                                {
                                    new SolutionFolderInfo
                                    {
                                        Name = "Client"
                                    },
                                    new ProjectInfo
                                    {
                                        ProjectName = "$safeprojectname$.Client.Presentation.Shared",
                                        FileName = @"Specifications\Samples.Specifications.Client.Presentation.Shared\MyTemplate.vstemplate"
                                    }
                                }
                            }
                        }
                    },
                    new SolutionInfo
                    {
                        Name = "Samples.Universal",
                        Caption = "UWP Application",
                        IconName = "",
                    },
                },
            };
        }
    }
}
