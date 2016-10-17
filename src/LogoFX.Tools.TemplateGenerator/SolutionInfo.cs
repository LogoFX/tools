using System.Xml.Serialization;
using LogoFX.Tools.TemplateGenerator.Contracts;

namespace LogoFX.Tools.TemplateGenerator
{
    public sealed class SolutionInfo
    {
        public SolutionInfo()
        {
            SolutionVariants = new SolutionVariant[0];
        }

        public string Name { get; set; }
        public string Caption { get; set; }
        public string IconName { get; set; }
        public string PostCreateUrl { get; set; }
        public string StartupProjectName { get; set; }
        public SolutionVariant[] SolutionVariants { get; set; }

        [XmlIgnore]
        public ISolutionTemplateInfo[] SolutionTemplateInfos { get; set; }
    }
}