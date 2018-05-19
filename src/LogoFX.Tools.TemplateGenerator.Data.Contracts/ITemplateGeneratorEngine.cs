using System.Threading.Tasks;

namespace LogoFX.Tools.TemplateGenerator.Data.Contracts
{
    public interface ITemplateGeneratorEngine
    {
        Task<ISolutionInfo> CreateSolutionInfo(string solutionFilename);
    }
}