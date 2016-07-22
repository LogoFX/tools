using System;
using AutoMapper;
using $saferootprojectname$.Client.Data.Contracts.Dto;
using $safeprojectname$.Contracts;

namespace $safeprojectname$.Mappers
{
    class MappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateWarehouseMaps();
        }

        private void CreateWarehouseMaps()
        {
            CreateDomainObjectMap<WarehouseItemDto, IWarehouseItem, WarehouseItem>();
        }

        //TODO: put this piece of functionality into 
        //an external package, e.g. Model.Mapping.AutoMapper
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
