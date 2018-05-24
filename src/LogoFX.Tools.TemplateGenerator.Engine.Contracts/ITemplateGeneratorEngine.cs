using System.Threading.Tasks;
using System.Xml.Linq;
using LogoFX.Tools.TemplateGenerator.Data.Contracts.Dto;
using LogoFX.Tools.TemplateGenerator.Engine.Shared;

namespace LogoFX.Tools.TemplateGenerator.Engine.Contracts
{
    public interface ITemplateGeneratorEngine
    {
        string Name { get; }

        Task<SolutionTemplateInfo> CreateSolutionInfoAsync(SolutionConfigurationDto solutionConfiguration);

        XDocument CreateDefinitionDocument(SolutionConfigurationDto solutionConfiguration);

        XDocument CreatePreprocessDocument(SolutionConfigurationDto solutionConfiguration);
    }
}