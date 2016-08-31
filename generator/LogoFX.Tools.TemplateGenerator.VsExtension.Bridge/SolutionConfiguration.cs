using LogoFX.Tools.TemplateGenerator.Contracts;

namespace LogoFX.Tools.TemplateGenerator.VsExtension.Bridge
{
    public sealed class SolutionConfiguration : ISolutionConfiguration
    {
        public string FileName { get; set; }
        public string DestinationPath { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DefaultName { get; set; }
    }
}