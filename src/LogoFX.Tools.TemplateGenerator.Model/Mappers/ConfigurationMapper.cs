using AutoMapper;
using LogoFX.Tools.TemplateGenerator.Data.Contracts.Dto;
using IConfiguration = LogoFX.Tools.TemplateGenerator.Model.Contract.IConfiguration;

namespace LogoFX.Tools.TemplateGenerator.Model.Mappers
{
    internal static class ConfigurationMapper
    {
        public static Configuration MapToConfiguration(ConfigurationDto configurationDto)
        {
            return Mapper.Map<Configuration>(configurationDto);
        }

        public static ProjectConfiguration MapToProjectConfiguration(ProjectConfigurationDto projectConfigurationDto)
        {
            return Mapper.Map<ProjectConfiguration>(projectConfigurationDto);
        }

        public static ConfigurationDto MapFromConfiguration(IConfiguration configuration)
        {
            return Mapper.Map<ConfigurationDto>(configuration);
        }
    }
}