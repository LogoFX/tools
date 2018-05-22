using AutoMapper;
using LogoFX.Tools.TemplateGenerator.Data.Contracts.Dto;

namespace LogoFX.Tools.TemplateGenerator.Model.Mappers
{
    internal static class InfoMapper
    {
        public static TemplateGeneratorEngineInfo MapToTemplateGeneratorEngineInfo(TemplateGeneratorEngineInfoDto dto)
        {
            return Mapper.Map<TemplateGeneratorEngineInfo>(dto);
        }
    
        public static ProjectInfo MapToProjectInfo(ProjectInfoDto projectInfoDto)
        {
            return Mapper.Map<ProjectInfo>(projectInfoDto);
        }

    }
}