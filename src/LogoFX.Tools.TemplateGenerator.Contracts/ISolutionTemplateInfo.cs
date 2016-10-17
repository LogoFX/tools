using System.Collections.Generic;

namespace LogoFX.Tools.TemplateGenerator.Contracts
{
    public interface ISolutionTemplateInfo : ISolutionFolderTemplateInfo
    {
        string ContainerName { get; }
        ICollection<string> RootNamespaces { get; }
    }
}