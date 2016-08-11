using System.Collections.Generic;

namespace LogoFX.Tools.TemplateGenerator
{
    public interface ISolutionFolderTemplateInfo : ISolutionItemTemplateInfo
    {
        IEnumerable<ISolutionItemTemplateInfo> Items { get; }
    }
}