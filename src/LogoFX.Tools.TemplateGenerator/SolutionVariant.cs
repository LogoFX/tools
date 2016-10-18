using System.Xml.Serialization;
using LogoFX.Tools.TemplateGenerator.Contracts;

namespace LogoFX.Tools.TemplateGenerator
{
    public sealed class SolutionVariant
    {
        public string ContainerName { get; set; }
        public string SolutionFileName { get; set; }
        public string StartupProjectName { get; set; }

        [XmlIgnore]
        public ISolutionTemplateInfo SolutionTemplateInfo { get; set; }
    }
}