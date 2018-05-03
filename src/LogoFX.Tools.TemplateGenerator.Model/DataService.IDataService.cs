﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using LogoFX.Tools.TemplateGenerator.Model.Contract;

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

        void IDataService.RemoveSolution(ISolutionConfiguration solution)
        {
            _configuration.Solutions.Remove((SolutionConfiguration) solution);
        }

        void IDataService.SetSolutionPath(ISolutionConfiguration solution, string path)
        {
            ((SolutionConfiguration) solution).Path = path;
        }

        public IEnumerable<ISolutionConfigurationPlugin> GetAvailablePlugins()
        {
            if (_plugins == null)
            {
                _plugins = IoC.GetAll<ISolutionConfigurationPlugin>();
            }

            return _plugins;
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