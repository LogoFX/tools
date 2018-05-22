using System;
using System.Threading.Tasks;
using LogoFX.Tools.TemplateGenerator.Data.Contracts.Dto;

namespace LogoFX.Tools.TemplateGenerator.Engine.Contracts
{
    public interface ITemplateGeneratorService
    {
        TemplateGeneratorEngineInfoDto[] GetAvailableEngines();

        Task<ProjectInfoDto[]> GetProjects(SolutionConfigurationDto solutionConfiguration, Guid engineId);

    }
}