using System.Xml.Serialization;

namespace LogoFX.Tools.Common.Model
{
    [XmlInclude(typeof(SolutionFolder))]
    [XmlInclude(typeof(ProjectData))]
    public abstract class SolutionItemData
    {
        public string Name { get; set; }
    }
}