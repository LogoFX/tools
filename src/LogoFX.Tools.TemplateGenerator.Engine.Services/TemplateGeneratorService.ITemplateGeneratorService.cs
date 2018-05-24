using System;
using System.Linq;
using System.Threading.Tasks;
using LogoFX.Tools.TemplateGenerator.Data.Contracts.Dto;
using LogoFX.Tools.TemplateGenerator.Engine.Contracts;

namespace LogoFX.Tools.TemplateGenerator.Engine.Services
{
    internal sealed partial class TemplateGeneratorService
    {
        TemplateGeneratorEngineInfoDto[] ITemplateGeneratorService.GetAvailableEngines()
        {
            GetOrCreateEngines();

            return _engines
                .Select(x => new TemplateGeneratorEngineInfoDto
                {
                    Id = x.Key,
                    Name = x.Value.Name
                })
                .ToArray();
        }

        Task ITemplateGeneratorService.GenerateAsync(SolutionConfigurationDto solutionConfiguration, Guid engineId, IProgress<double> progress)
        {
            return GenerateInternalAsync(solutionConfiguration, engineId, progress);
        }
    }
}