using System;
using System.IO;
using System.Xml.Serialization;
using JetBrains.Annotations;
using LogoFX.Tools.TemplateGenerator.Contracts;

namespace LogoFX.Tools.TemplateGenerator.Launcher
{
    [UsedImplicitly]
    internal sealed class DataService : IDataService
    {
        private readonly string _configFileName;
        private Configuration _configuration;

        public DataService()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            path = Path.Combine(path, "TemplateGenerator");
            _configFileName = Path.Combine(path, "TemplateGenerator.cfg");
        }

        public string GetSolutionFileName()
        {
            return @"c:\Projects\LogoUI\LogoFX\Samples.Specifications\Specifications.sln";
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
                        _configuration = (Configuration)serializer.Deserialize(sr);
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
    }
}