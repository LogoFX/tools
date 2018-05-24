using System.Collections.Generic;
using JetBrains.Annotations;

namespace LogoFX.Tools.TemplateGenerator.Data.Contracts.Dto
{
    [UsedImplicitly]
    public sealed class ConfigurationDto
    {
        public static ConfigurationVersion CurrentVersion => new ConfigurationVersion(2);

        public List<SolutionConfigurationDto> Solutions { get; set; }
    }
}