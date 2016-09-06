using System;

namespace LogoFX.Tools.TemplateGenerator.Contracts
{
    public interface ISolutionItemTemplateInfo
    {
        Guid Id { get; }
        string Name { get; }
    }
}