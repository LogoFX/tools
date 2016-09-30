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

        public bool IsMultisolution { get; set; }

        public List<SolutionInfo> Solutions { get; set; }
    }

    public sealed class SolutionInfo
    {
        public SolutionInfo()
        {
            Items = new List<SolutionItemInfo>();
        }

        public string FileName { get; set; }
        public string Name { get; set; }
        public string Caption { get; set; }
        public string IconName { get; set; }
        public List<SolutionItemInfo> Items { get; set; }
    }

    public abstract class SolutionItemInfo
    {
    }

    public sealed class SolutionFolderInfo : SolutionItemInfo
    {
        public SolutionFolderInfo()
        {
            Items = new List<SolutionItemInfo>();
        }

        public string Name { get; set; }

        public List<SolutionItemInfo> Items { get; set; }
    }

    public sealed class ProjectInfo : SolutionItemInfo
    {
        public string ProjectName { get; set; }
        public string FileName { get; set; }
    }
}