using System;
using System.Threading.Tasks;

namespace LogoFX.Tools.TemplateGenerator.Model.Contract
{
    public interface IDataService
    {
        Task<IConfiguration> GetConfigurationAsync();

        Task SaveConfigurationAsync();

        ISolutionConfiguration AddSolution(string name);

        void RemoveSolution(ISolutionConfiguration solution);

        void SetSolutionPath(ISolutionConfiguration solution, string path);

        ITemplateGeneratorEngineInfo[] GetAvailableEngines();

        Task GenerateAsync(ISolutionConfiguration solutionConfiguration, ITemplateGeneratorEngineInfo engine, IProgress<double> progress);
    }
}