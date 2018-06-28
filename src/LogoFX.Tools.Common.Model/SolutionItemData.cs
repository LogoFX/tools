using System.Xml.Serialization;
using JetBrains.Annotations;

namespace LogoFX.Tools.Common.Model
{
    [XmlInclude(typeof(SolutionFolderData))]
    [XmlInclude(typeof(ProjectData))]
    public abstract class SolutionItemData
    {
        public string Name { get; [UsedImplicitly] set; }
    }
}