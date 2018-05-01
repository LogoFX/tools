using System;
using System.Threading.Tasks;

namespace LogoFX.Tools.TemplateGenerator.Model.Contract
{
    public interface IDataService
    {
        Task<IConfiguration> GetConfigurationAsync();

        Task SaveConfigurationAsync();

        ISolutionConfiguration AddSolution(string path);

        void RemoveSolution(ISolutionConfiguration solution);

        Task GenerateTemplates(ISolutionConfiguration[] solutions, IProgress<double> progress);
    }
}