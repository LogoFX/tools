using LogoFX.Tools.TemplateGenerator.Contracts;

namespace LogoFX.Tools.TemplateGenerator
{
    internal sealed class ProjectConfiguration : IProjectConfiguration
    {
        public string ConfigurationName { get; set; }
        public string PlatformName { get; set; }
        public string Name { get; set; }
        public bool IncludeInBuild { get; set; }
    }
}