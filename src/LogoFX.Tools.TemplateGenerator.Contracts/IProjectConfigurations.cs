using System.Collections.Generic;

namespace LogoFX.Tools.TemplateGenerator.Contracts
{
    public interface IProjectConfigurations
    {
        string Name { get; }

        IEnumerable<IProjectConfiguration> Configurations { get; }
    }
}