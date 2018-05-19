using System.Linq;
using LogoFX.Tools.TemplateGenerator.Data.Contracts.Dto;
using NDatabase;

namespace LogoFX.Tools.TemplateGenerator.Data.Real.Providers
{
    internal static class StoreHelper
    {
        private static string ConfigurationDatabaseName => "logofx.tools.templategenerator.db";

        public static ConfigurationDto LoadConfiguration()
        {
            using (var odb = OdbFactory.Open(ConfigurationDatabaseName))
            {
                var version = odb.Query<ConfigurationVersion>();

                if (version.Count() != 1)
                {
                    return null;
                }

                var v = version.Execute<ConfigurationVersion>().GetFirst();
                if (v != ConfigurationDto.CurrentVersion)
                {
                    return null;
                }

                var result = odb.Query<ConfigurationDto>();
                if (result.Count() == 0)
                {
                    return null;
                }

                var configDto = result.Execute<ConfigurationDto>().FirstOrDefault();
                return configDto;
            }
        }

        public static void SaveConfiguration(ConfigurationDto configuration)
        {
            OdbFactory.Delete(ConfigurationDatabaseName);
            using (var odb = OdbFactory.Open(ConfigurationDatabaseName))
            {
                odb.Store(ConfigurationDto.CurrentVersion);
                odb.Store(configuration);
            }
        }
    }
}