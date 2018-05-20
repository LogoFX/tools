using System.Linq;
using Caliburn.Micro;
using JetBrains.Annotations;
using LogoFX.Tools.TemplateGenerator.Data.Contracts.Dto;
using LogoFX.Tools.TemplateGenerator.Engine.Contracts;

namespace LogoFX.Tools.TemplateGenerator.Engine.Services
{
    [UsedImplicitly]
    internal sealed class TemplateGeneratorService : ITemplateGeneratorService
    {
        public TemplateGeneratorEngineInfoDto[] GetAvailableEngines()
        {
            return IoC.GetAll<ITemplateGeneratorEngine>()
                .Select(x => new TemplateGeneratorEngineInfoDto
                {
                    Name = x.Name
                })
                .ToArray();
        }
    }
}