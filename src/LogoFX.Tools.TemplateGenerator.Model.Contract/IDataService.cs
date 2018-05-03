using System;
using System.Collections.Generic;
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

        IEnumerable<ISolutionConfigurationPlugin> GetAvailablePlugins();
        
        Task GenerateTemplates(ISolutionConfiguration[] solutions, IProgress<double> progress);
    }
}