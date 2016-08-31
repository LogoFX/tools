using System;
using System.Collections.Generic;
using System.IO;
using LogoFX.Tools.TemplateGenerator.Contracts;

namespace LogoFX.Tools.TemplateGenerator.VsExtension.Bridge
{
    public sealed class Configuration : IConfiguration
    {
        private List<SolutionConfiguration> _solutionConfigurations =
            new List<SolutionConfiguration>();

        public IEnumerable<ISolutionConfiguration> SolutionConfigurations { get; }
        public ISolutionConfiguration CreateNewSolutionConfiguration(string solutionFullName)
        {
            throw new NotImplementedException();
        }

        public void AddSolutionConfiguration(ISolutionConfiguration solutionConfiguration)
        {
            throw new NotImplementedException();
        }
    }
}