using System;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LogoFX.Tools.TemplateGenerator.Model.Contract;

namespace LogoFX.Tools.TemplateGenerator.Model
{
    [UsedImplicitly]
    internal sealed class DataService : IDataService
    {
        private readonly object _loadConfigurationSync = new object();
        private Configuration _configuration;

        private Task LoadOrCreateConfigurationAsync()
        {
            return Task.Run(() =>
            {
                lock (_loadConfigurationSync)
                {
                    if (_configuration != null)
                    {
                        return;
                    }

                    //TODO: Add configuration load code here
                    _configuration = new Configuration();
                }
            });
        }

        async Task<IConfiguration> IDataService.GetConfigurationAsync()
        {
            if (_configuration == null)
            {
                await LoadOrCreateConfigurationAsync();
            }

            lock (_loadConfigurationSync)
            {
                return _configuration;
            }
        }

        public ISolutionConfiguration AddSolution(string name)
        {
            if (_configuration.Solutions.Any(x => x.Name == name))
            {
                throw new ArgumentException($"This name already added to Configuration: {name}", nameof(name));
            }

            var solutionConfiguration = new SolutionConfiguration
            {
                Name = name
            };

            _configuration.Solutions.Add(solutionConfiguration);

            return solutionConfiguration;
        }

        public void RemoveSolution(ISolutionConfiguration solution)
        {
            _configuration.Solutions.Remove((SolutionConfiguration) solution);
        }

        public void SetSolutionPath(ISolutionConfiguration solution, string path)
        {
            ((SolutionConfiguration) solution).Path = path;
        }

        Task IDataService.SaveConfigurationAsync()
        {
            throw new NotImplementedException();
        }

        Task IDataService.GenerateTemplates(ISolutionConfiguration[] solutions, IProgress<double> progress)
        {
            throw new NotImplementedException();
        }
    }
}