using System.Threading.Tasks;

namespace LogoFX.Tools.TemplateGenerator.Engine.Contracts
{
    public interface ITemplateGeneratorEngine
    {
        Task<ISolutionInfo> CreateSolutionInfo(string solutionFilename);
    }
}