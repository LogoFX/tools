using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LogoFX.Tools.TemplateGenerator
{
    public static class WizardConfigurator
    {
        private const string CfgFileName = "WizardConfiguration.xml";

        public static string GetWizardConfigurationFileName(string path)
        {
            return Path.Combine(path, CfgFileName);
        }

        public static Task<WizardConfiguration> LoadAsync(string fileName)
        {
            return Task.Run(() =>
            {
                XmlSerializer serializer = new XmlSerializer(typeof(WizardConfiguration));
                using (var tr = File.OpenText(fileName))
                {
                    return (WizardConfiguration) serializer.Deserialize(tr);
                }
            });
        }

        public static Task SaveAsync(string fileName, WizardConfiguration wizardConfiguration)
        {
            return Task.Run(() =>
            {
                XmlSerializer serializer = new XmlSerializer(typeof(WizardConfiguration));
                using (var tw = File.CreateText(fileName))
                {
                    serializer.Serialize(tw, wizardConfiguration);
                }
            });
        }
    }
}