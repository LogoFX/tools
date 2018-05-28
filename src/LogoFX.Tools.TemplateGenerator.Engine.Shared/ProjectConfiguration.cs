namespace LogoFX.Tools.TemplateGenerator.Engine.Shared
{
    public sealed class ProjectConfiguration
    {
        public string ConfigurationName { get; set; }
        public string PlatformName { get; set; }
        public string Name { get; set; }
        public bool IncludeInBuild { get; set; }
    }
}