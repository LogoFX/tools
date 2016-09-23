using System.Collections.Generic;
using System.Linq;
using LogoFX.Tools.TemplateGenerator.Contracts;

namespace LogoFX.Tools.TemplateGenerator
{
    public sealed class Configuration : IConfiguration
    {
        IEnumerable<ISolutionConfiguration> IConfiguration.SolutionConfigurations => SolutionConfigurations;

        public List<SolutionConfiguration> SolutionConfigurations { get; set; }

        public ISolutionConfiguration CreateNewSolutionConfiguration(string solutionFullName)
        {
            var solutionConfiguration = new SolutionConfiguration
            {
                FileName = solutionFullName
            };

            if (SolutionConfigurations == null)
            {
                SolutionConfigurations = new List<SolutionConfiguration>();
            }

            SolutionConfigurations.Add(solutionConfiguration);
            return solutionConfiguration;
        }

        public void RemoveSolutionConfiguration(string solutionFileName)
        {
            var solutionConfiguration = SolutionConfigurations.Single(x => x.FileName == solutionFileName);
            SolutionConfigurations.Remove(solutionConfiguration);
        }
    }
}