using System.Xml.Serialization;

namespace LogoFX.Tools.Common.Model
{
    [XmlInclude(typeof(SolutionFolderData))]
    [XmlInclude(typeof(ProjectData))]
    public abstract class SolutionItemData
    {
        public string Name { get; set; }
    }
}