using AutoMapper;
using JetBrains.Annotations;
using LogoFX.Tools.TemplateGenerator.Model.Mappers;
using Solid.Practices.Modularity;

namespace LogoFX.Tools.TemplateGenerator.Model
{
    [UsedImplicitly]
    internal sealed class MappingModule : IPlainCompositionModule
    {
        public void RegisterModule()
        {            
            Mapper.Initialize(x => x.AddProfile<MappingProfile>());
        }
    }
}