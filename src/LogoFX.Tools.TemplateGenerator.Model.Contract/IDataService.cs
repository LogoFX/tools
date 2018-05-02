using System;
using System.Threading.Tasks;

namespace LogoFX.Tools.TemplateGenerator.Model.Contract
{
    public interface IDataService
    {
        Task<IConfiguration> GetConfigurationAsync();

        ISolutionConfiguration AddSolution(string name);

        void RemoveSolution(ISolutionConfiguration solution);

        void SetSolutionPath(ISolutionConfiguration solution, string path);

        Task SaveConfigurationAsync();

        Task GenerateTemplates(ISolutionConfiguration[] solutions, IProgress<double> progress);
    }
}