using System.Configuration;
using JetBrains.Annotations;
using RestSharp;
using $saferootprojectname$.lient.Data.Contracts.Providers;
using Solid.Practices.IoC;
using Solid.Practices.Modularity;

namespace $safeprojectname$
{    
    [UsedImplicitly]
    class Module : ICompositionModule<IDependencyRegistrator>
    {
        public void RegisterModule(IDependencyRegistrator dependencyRegistrator)
        {
            dependencyRegistrator.RegisterSingleton<ILoginProvider, LoginProvider>();
            dependencyRegistrator.RegisterSingleton<IWarehouseProvider, WarehouseProvider>();
            dependencyRegistrator.RegisterSingleton<IEventsProvider, EventsProvider>();
            dependencyRegistrator.RegisterSingleton(() => new RestClient(RetrieveEndpoint()));
        }

        private string RetrieveEndpoint()
        {            
            var exeConfigPath = GetType().Assembly.Location;
            var config = ConfigurationManager.OpenExeConfiguration(exeConfigPath);
            return GetAppSetting(config, "ServerEndpoint");
        }

        private string GetAppSetting(Configuration config, string key)
        {
            var element = config.AppSettings.Settings[key];
            var value = element?.Value;
            return string.IsNullOrEmpty(value) ? string.Empty : value;
        }
    }


}
