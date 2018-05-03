using System.Threading.Tasks;
using JetBrains.Annotations;
using LogoFX.Tools.TemplateGenerator.Model.Contract;

namespace LogoFX.Tools.TemplateGenerator.Model
{
    [UsedImplicitly]
    internal sealed partial class DataService : IDataService
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
    }
}