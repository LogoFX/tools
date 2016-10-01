using System.Collections.ObjectModel;
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

    public sealed class WizardConfiguration
    {
        public WizardConfiguration()
        {
            Solutions = new ObservableCollection<SolutionInfo>();
        }

        public ObservableCollection<SolutionInfo> Solutions { get; }

        public string ProjectType => "CSharp";

        public int SortOrder => 5000;
    }
}