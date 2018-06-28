using JetBrains.Annotations;

namespace LogoFX.Tools.Common.Model
{
    public class ProjectData : SolutionItemData
    {
        public string FileName { get; [UsedImplicitly] set; }

        public bool IsStartup { get; [UsedImplicitly] set; }

        public ProjectConfigurationData[] ProjectConfigurations { get; [UsedImplicitly] set; }
    }
}