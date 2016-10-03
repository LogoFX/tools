using System.Collections.Generic;

namespace LogoFX.Tools.TemplateGenerator.Contracts
{
    public interface ISolutionTemplateInfo : ISolutionFolderTemplateInfo
    {
        ICollection<string> RootNamespaces { get; }
    }
}