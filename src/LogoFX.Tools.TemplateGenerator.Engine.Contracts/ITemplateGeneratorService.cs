using System;
using System.Threading.Tasks;
using LogoFX.Tools.TemplateGenerator.Data.Contracts.Dto;

namespace LogoFX.Tools.TemplateGenerator.Engine.Contracts
{
    public interface ITemplateGeneratorService
    {
        TemplateGeneratorEngineInfoDto[] GetAvailableEngines();

        Task GenerateAsync(SolutionConfigurationDto solutionConfiguration, Guid engineId, IProgress<double> progress);
    }
}