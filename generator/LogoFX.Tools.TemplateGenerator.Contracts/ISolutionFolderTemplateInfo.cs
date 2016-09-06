using System.Collections.Generic;

namespace LogoFX.Tools.TemplateGenerator.Contracts
{
    public interface ISolutionFolderTemplateInfo : ISolutionItemTemplateInfo
    {
        IEnumerable<ISolutionItemTemplateInfo> Items { get; }
    }
}