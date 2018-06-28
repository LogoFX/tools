using JetBrains.Annotations;

namespace LogoFX.Tools.Common.Model
{
    public sealed class ProjectConfigurationData
    {
        public string Name { get; [UsedImplicitly] set; }
        public string ConfigurationName { get; [UsedImplicitly] set; }
        public string PlatformName { get; set; }
        public bool IncludeInBuild { get; [UsedImplicitly] set; }
    }
}