using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LogoFX.Tools.TemplateGenerator.Engine.Contracts;

namespace LogoFX.Tools.TemplateGenerator.Model.Contract
{
    public interface IDataService
    {
        Task<IConfiguration> GetConfigurationAsync();

        Task SaveConfigurationAsync();

        ISolutionConfiguration AddSolution(string name);

        void RemoveSolution(ISolutionConfiguration solution);

        void SetSolutionPath(ISolutionConfiguration solution, string path);

        IEnumerable<ITemplateGeneratorEngine> GetAvailableEngines();

        IProjectConfiguration[] GetProjectConfigurations(ISolutionConfiguration solutionConfiguration, ITemplateGeneratorEngine engine);
        
        Task GenerateTemplates(ISolutionConfiguration[] solutions, IProgress<double> progress);
    }
}