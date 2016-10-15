using System.Xml.Serialization;
using LogoFX.Tools.TemplateGenerator.Contracts;

namespace LogoFX.Tools.TemplateGenerator
{
    public sealed class SolutionInfo
    {
        public string FileName { get; set; }
        public string Name { get; set; }
        public string Caption { get; set; }
        public string IconName { get; set; }
        public string PostCreateUrl { get; set; }
        public string StartupProjectName { get; set; }

        [XmlIgnore]
        public ISolutionTemplateInfo SolutionTemplateInfo { get; set; }
    }
}