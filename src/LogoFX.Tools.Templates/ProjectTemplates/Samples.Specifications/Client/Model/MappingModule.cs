using AutoMapper;
using JetBrains.Annotations;
using $safeprojectname$.Mappers;
using Solid.Practices.Modularity;

namespace $safeprojectname$
{   
    [UsedImplicitly]
    class MappingModule : IPlainCompositionModule
    {
        public void RegisterModule()
        {
            Mapper.Initialize(x => x.AddProfile<MappingProfile>());
        }
    }
}