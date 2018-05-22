using AutoMapper;
using LogoFX.Tools.TemplateGenerator.Data.Contracts.Dto;
using LogoFX.Tools.TemplateGenerator.Model.Contract;
using IConfiguration = LogoFX.Tools.TemplateGenerator.Model.Contract.IConfiguration;

namespace LogoFX.Tools.TemplateGenerator.Model.Mappers
{
    internal static class ConfigurationMapper
    {
        public static Configuration MapToConfiguration(ConfigurationDto configurationDto)
        {
            return Mapper.Map<Configuration>(configurationDto);
        }

        public static ConfigurationDto MapFromConfiguration(IConfiguration configuration)
        {
            return Mapper.Map<ConfigurationDto>(configuration);
        }

        public static SolutionConfigurationDto MapFromSolutionConfiguration(ISolutionConfiguration solutionConfiguration)
        {
            return Mapper.Map<SolutionConfigurationDto>(solutionConfiguration);
        }
    }
}