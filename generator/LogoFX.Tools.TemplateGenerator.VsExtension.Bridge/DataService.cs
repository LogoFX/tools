using System;
using System.IO;
using System.Xml.Serialization;
using LogoFX.Tools.TemplateGenerator.Contracts;

namespace LogoFX.Tools.TemplateGenerator.VsExtension.Bridge
{
    internal sealed class DataService : IDataService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly string _configFileName;
        private Configuration _configuration;

        public DataService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            path = Path.Combine(path, "TemplateGenerator");
            _configFileName = Path.Combine(path, "TemplateGenerator.cfg");
        }

        public string GetSolutionFileName()
        {
            var dte = (EnvDTE.DTE) _serviceProvider.GetService(typeof(EnvDTE.DTE));
            return dte?.Solution?.FullName;
        }

        public IConfiguration LoadConfiguration()
        {
            if (_configuration == null)
            {
                if (File.Exists(_configFileName))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
                    using (var sr = File.OpenText(_configFileName))
                    {
                        _configuration = (Configuration) serializer.Deserialize(sr);
                    }
                }
                else
                {
                    _configuration = new Configuration();
                }
            }

            return _configuration;
        }

        public void SaveConfiguration(IConfiguration configuration)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
            var path = Path.GetDirectoryName(_configFileName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (var sw = File.CreateText(_configFileName))
            {
                serializer.Serialize(sw, configuration);
            }
        }

        public bool ShowInTaskbar => false;
    }
}