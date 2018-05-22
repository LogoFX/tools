using System.Threading.Tasks;
using LogoFX.Tools.TemplateGenerator.Engine.Shared;

namespace LogoFX.Tools.TemplateGenerator.Engine.Contracts
{
    public interface ITemplateGeneratorEngine
    {
        string Name { get; }

        Task<SolutionTemplateInfo> CreateSolutionInfoAsync(string solutionFileName);
    }
}