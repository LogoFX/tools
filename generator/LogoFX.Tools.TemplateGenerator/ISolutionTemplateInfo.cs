using System.Collections.Generic;

namespace LogoFX.Tools.TemplateGenerator
{
    public interface ISolutionTemplateInfo : ISolutionFolderTemplateInfo
    {
        ICollection<string> RootNamespaces { get; }
    }
}