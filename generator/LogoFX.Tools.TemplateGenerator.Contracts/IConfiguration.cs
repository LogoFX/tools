using System.Collections.Generic;

namespace LogoFX.Tools.TemplateGenerator.Contracts
{
    public interface IConfiguration
    {
        IEnumerable<ISolutionConfiguration> SolutionConfigurations { get; }

        ISolutionConfiguration CreateNewSolutionConfiguration(string solutionFullName);

        void AddSolutionConfiguration(ISolutionConfiguration solutionConfiguration);
    }
}