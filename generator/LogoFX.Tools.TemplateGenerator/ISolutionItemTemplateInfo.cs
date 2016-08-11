using System;
using System.Security.Cryptography.X509Certificates;

namespace LogoFX.Tools.TemplateGenerator
{
    public interface ISolutionItemTemplateInfo
    {
        Guid Id { get; }
        string Name { get; }
    }
}