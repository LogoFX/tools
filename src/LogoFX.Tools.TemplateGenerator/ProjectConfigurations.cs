using System.Collections.Generic;
using LogoFX.Tools.TemplateGenerator.Contracts;

namespace LogoFX.Tools.TemplateGenerator
{
    internal sealed class ProjectConfigurations : IProjectConfigurations
    {
        public string Name { get; }
        public IEnumerable<IProjectConfiguration> Configurations { get; }
    }
}