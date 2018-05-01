using System;
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

        Task IDataService.SaveConfigurationAsync()
        {
            throw new NotImplementedException();
        }

        ISolutionConfiguration IDataService.AddSolution(string path)
        {
            throw new NotImplementedException();
        }

        void IDataService.RemoveSolution(ISolutionConfiguration solution)
        {
            throw new NotImplementedException();
        }

        Task IDataService.GenerateTemplates(ISolutionConfiguration[] solutions, IProgress<double> progress)
        {
            throw new NotImplementedException();
        }
    }
}