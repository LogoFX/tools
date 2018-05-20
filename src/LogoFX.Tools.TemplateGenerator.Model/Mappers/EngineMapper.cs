using AutoMapper;
using LogoFX.Tools.TemplateGenerator.Data.Contracts.Dto;

namespace LogoFX.Tools.TemplateGenerator.Model.Mappers
{
    internal static class EngineMapper
    {
        public static TemplateGeneratorEngineInfo MapToTemplateGeneratorEngineInfo(TemplateGeneratorEngineInfoDto dto)
        {
            return Mapper.Map<TemplateGeneratorEngineInfo>(dto);
        }
    }
}