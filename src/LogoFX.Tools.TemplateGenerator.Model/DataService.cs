using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LogoFX.Tools.TemplateGenerator.Data.Contracts.Providers;
using LogoFX.Tools.TemplateGenerator.Engine.Contracts;
using LogoFX.Tools.TemplateGenerator.Model.Contract;
using LogoFX.Tools.TemplateGenerator.Model.Mappers;

namespace LogoFX.Tools.TemplateGenerator.Model
{
    [UsedImplicitly]
    internal sealed partial class DataService : IDataService
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IEngineProvider _engineProvider;
        private readonly object _loadConfigurationSync = new object();
        private Configuration _configuration;
        private IEnumerable<ITemplateGeneratorEngine> _plugins;

        public DataService(
            IConfigurationProvider configurationProvider,
            IEngineProvider engineProvider)
        {
            _configurationProvider = configurationProvider;
            _engineProvider = engineProvider;
        }

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

                    _configuration = ConfigurationMapper.MapToConfiguration(_configurationProvider.GetConfiguration());
                }
            });
        }
    }
}