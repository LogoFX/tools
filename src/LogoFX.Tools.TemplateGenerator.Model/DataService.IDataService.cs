using System;
using System.IO;
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
            var solutionConfiguration = (SolutionConfiguration) solution;
            solutionConfiguration.Path = path;
            if (string.IsNullOrWhiteSpace(solutionConfiguration.Name))
            {
                solutionConfiguration.Name = Path.GetFileNameWithoutExtension(path);
            }
        }

        ITemplateGeneratorEngineInfo[] IDataService.GetAvailableEngines()
        {
            return _templateGeneratorService
                .GetAvailableEngines()
                .Select(InfoMapper.MapToTemplateGeneratorEngineInfo)
                .OfType<ITemplateGeneratorEngineInfo>()
                .ToArray();
        }

        Task IDataService.GenerateAsync(ISolutionConfiguration solutionConfiguration, ITemplateGeneratorEngineInfo engine, IProgress<double> progress)
        {
            var solutionConfigurationDto = ConfigurationMapper.MapFromSolutionConfiguration(solutionConfiguration);
            return _templateGeneratorService.GenerateAsync(solutionConfigurationDto, engine.Id, progress);
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
    }
}