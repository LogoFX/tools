using System.Collections.Generic;
using JetBrains.Annotations;
using LogoFX.Tools.TemplateGenerator.Data.Contracts.Dto;
using LogoFX.Tools.TemplateGenerator.Data.Contracts.Providers;

namespace LogoFX.Tools.TemplateGenerator.Data.Real.Providers
{
    [UsedImplicitly]
    internal sealed class ConfigurationProvider : IConfigurationProvider
    {
        public ConfigurationDto GetConfiguration()
        {
            var configuration = StoreHelper.LoadConfiguration() ??
                                new ConfigurationDto()
                                {
                                    Solutions = new List<SolutionConfigurationDto>()
                                };
            return configuration;
        }

        public void SaveConfiguration(ConfigurationDto configurationDto)
        {
            StoreHelper.SaveConfiguration(configurationDto);
        }
    }
}