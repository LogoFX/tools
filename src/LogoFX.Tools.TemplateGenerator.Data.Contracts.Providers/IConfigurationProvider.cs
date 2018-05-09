using LogoFX.Tools.TemplateGenerator.Data.Contracts.Dto;

namespace LogoFX.Tools.TemplateGenerator.Data.Contracts.Providers
{
    public interface IConfigurationProvider
    {
        ConfigurationDto GetConfiguration();

        void SaveConfiguration(ConfigurationDto configurationDto);

    }
}