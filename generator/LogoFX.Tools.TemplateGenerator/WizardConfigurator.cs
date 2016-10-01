using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using LogoFX.Tools.Common;

namespace LogoFX.Tools.TemplateGenerator
{
    public static class WizardConfigurator
    {
        private const string CfgFileName = "WizardConfiguration.xml";

        public static string GetWizardConfigurationFileName(string path)
        {
            return Path.Combine(path, CfgFileName);
        }

        public static Task<WizardConfigurationDto> LoadAsync(string fileName)
        {
            return Task.Run(() =>
            {
                XmlSerializer serializer = new XmlSerializer(typeof(WizardConfigurationDto));
                using (var tr = File.OpenText(fileName))
                {
                    return (WizardConfigurationDto) serializer.Deserialize(tr);
                }
            });
        }

        public static Task SaveAsync(string fileName, WizardConfigurationDto wizardConfiguration)
        {
            return Task.Run(() =>
            {
                XmlSerializer serializer = new XmlSerializer(typeof(WizardConfigurationDto));
                using (var tw = File.CreateText(fileName))
                {
                    serializer.Serialize(tw, wizardConfiguration);
                }
            });
        }
    }
}