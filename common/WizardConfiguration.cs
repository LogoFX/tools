using System.Collections.Generic;

namespace LogoFX.Tools.Common
{
    public class WizardConfigurationDto
    {
        public WizardConfigurationDto()
        {
            Solutions = new List<SolutionInfoDto>();
        }

        public bool TestOption { get; set; }

        public bool FakeOption { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string DefaultName { get; set; }

        public string CodeFileName { get; set; }

        public List<SolutionInfoDto> Solutions { get; set; }

        public string IconName { get; set; }
    }

    public sealed class SolutionInfoDto
    {
        public string FileName { get; set; }
        public string Name { get; set; }
        public string Caption { get; set; }
        public string IconName { get; set; }
    }
}