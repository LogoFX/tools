using System.Collections.Generic;

namespace LogoFX.Tools.TemplateGenerator
{
    public sealed class WizardConfiguration
    {
        public WizardConfiguration()
        {
            Solutions = new List<SolutionInfo>();
        }

        public bool TestOption { get; set; }

        public bool FakeOption { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string DefaultName { get; set; }

        public string CodeFileName { get; set; }

        public List<SolutionInfo> Solutions { get; set; }
    }

    public sealed class SolutionInfo
    {
        public string Name { get; set; }
        public string Caption { get; set; }
        public string IconName { get; set; }
    }
}