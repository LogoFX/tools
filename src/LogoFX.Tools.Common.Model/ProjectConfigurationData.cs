namespace LogoFX.Tools.Common.Model
{
    public sealed class ProjectConfigurationData
    {
        public string Name { get; set; }
        public string ConfigurationName { get; set; }
        public string PlatformName { get; set; }
        public bool IncludeInBuild { get; set; }
    }
}