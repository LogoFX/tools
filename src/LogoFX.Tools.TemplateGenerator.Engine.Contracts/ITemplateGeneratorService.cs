using LogoFX.Tools.TemplateGenerator.Data.Contracts.Dto;

namespace LogoFX.Tools.TemplateGenerator.Engine.Contracts
{
    public interface ITemplateGeneratorService
    {
        TemplateGeneratorEngineInfoDto[] GetAvailableEngines();

        //IProjectConfiguration[] GetProjectConfigurations(ISolutionConfiguration solutionConfiguration, ITemplateGeneratorEngine engine);

    }
}