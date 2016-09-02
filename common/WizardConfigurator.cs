using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LogoFX.Tools.TemplateGenerator
{
    public sealed class WizardConfigurator
    {
        private const string CfgFileName = "WizardConfigureation.xml";

        public string GetWizardConfigurationFileName(string path)
        {
            return Path.Combine(path, CfgFileName);
        }

        public Task<WizardConfiguration> LoadAsync(string fileName)
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

        public Task SaveAsync(string fileName, WizardConfiguration wizardConfiguration)
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