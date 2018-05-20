using System;
using System.Linq;
using System.Threading.Tasks;
using LogoFX.Tools.TemplateGenerator.Model.Contract;
using LogoFX.Tools.TemplateGenerator.Model.Mappers;

namespace LogoFX.Tools.TemplateGenerator.Model
{
    internal sealed partial class DataService
    {
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

        ISolutionConfiguration IDataService.AddSolution(string name)
        {
            lock (_loadConfigurationSync)
            {
                if (_configuration.Solutions.Any(x => x.Name == name))
                {
                    throw new ArgumentException($"This name already added to Configuration: {name}", nameof(name));
                }
            }

            var solutionConfiguration = new SolutionConfiguration
            {
                Name = name
            };

            lock (_loadConfigurationSync)
            {
                _configuration.Solutions.Add(solutionConfiguration);
            }

            return solutionConfiguration;
        }

        void IDataService.RemoveSolution(ISolutionConfiguration solution)
        {
            lock (_loadConfigurationSync)
            {
                _configuration.Solutions.Remove((SolutionConfiguration) solution);
            }
        }

        void IDataService.SetSolutionPath(ISolutionConfiguration solution, string path)
        {
            ((SolutionConfiguration) solution).Path = path;
        }

        ITemplateGeneratorEngineInfo[] IDataService.GetAvailableEngines()
        {
            return _templateGeneratorService
                .GetAvailableEngines()
                .Select(EngineMapper.MapToTemplateGeneratorEngineInfo)
                .OfType<ITemplateGeneratorEngineInfo>()
                .ToArray();
        }

        Task IDataService.SaveConfigurationAsync()
        {
            return Task.Run(() =>
            {
                lock (_loadConfigurationSync)
                {
                    if (_configuration == null)
                    {
                        return;
                    }

                    _configurationProvider.SaveConfiguration(ConfigurationMapper.MapFromConfiguration(_configuration));
                }
            });
        }

        Task IDataService.GenerateTemplates(ISolutionConfiguration[] solutions, IProgress<double> progress)
        {
            throw new NotImplementedException();
        }
    }
}