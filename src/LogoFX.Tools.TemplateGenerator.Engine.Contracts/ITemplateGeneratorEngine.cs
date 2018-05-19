using System.Threading.Tasks;

namespace LogoFX.Tools.TemplateGenerator.Engine.Contracts
{
    public interface ITemplateGeneratorEngine
    {
        string Name { get; }

        Task<ISolutionInfo> CreateSolutionInfo(string solutionFilename);
    }
}