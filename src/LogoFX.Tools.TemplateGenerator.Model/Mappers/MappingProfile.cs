using System;
using AutoMapper;
using LogoFX.Tools.TemplateGenerator.Data.Contracts.Dto;

namespace LogoFX.Tools.TemplateGenerator.Model.Mappers
{
    internal sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateConfigurationMaps();
            CreateInfoMaps();
        }

        private void CreateInfoMaps()
        {
            CreateDomainObjectMap<TemplateGeneratorEngineInfoDto, Contract.ITemplateGeneratorEngineInfo, TemplateGeneratorEngineInfo>();
        }

        private void CreateConfigurationMaps()
        {
            CreateDomainObjectMap<SolutionConfigurationDto, Contract.ISolutionConfiguration, SolutionConfiguration>();
            CreateDomainObjectMap<ConfigurationDto, Contract.IConfiguration, Configuration>();
        }

        private void CreateDomainObjectMap<TDto, TContract, TModel>()
            where TModel : TContract
            where TContract : class
        {
            CreateDomainObjectMap(typeof (TDto), typeof (TContract), typeof (TModel));
        }

        private void CreateDomainObjectMap(Type dtoType, Type contractType, Type modelType)
        {
            CreateMap(dtoType, contractType).As(modelType);
            CreateMap(dtoType, modelType);
            CreateMap(contractType, dtoType);
            CreateMap(modelType, dtoType);
        }
    }
}