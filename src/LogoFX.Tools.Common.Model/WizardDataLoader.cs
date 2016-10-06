using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LogoFX.Tools.Common.Model
{
    public static class WizardDataLoader
    {
        public static string WizardDataFielName
        {
            get { return "WizardData.xml"; }
        }

        public static Task<WizardData> LoadAsync(string fileName)
        {
            return Task.Run(() =>
            {
                if (!File.Exists(fileName))
                {
                    return null;
                }

                var serializer = new XmlSerializer(typeof(WizardData));
                using (var fs = File.OpenRead(fileName))
                {
                    return (WizardData)serializer.Deserialize(fs);
                }
            });
        }

        public static Task SaveAsync(string fileName, WizardData data)
        {
            return Task.Run(() =>
            {
                var serializer = new XmlSerializer(typeof(WizardData));
                using (var fs = File.Create(fileName))
                {
                    serializer.Serialize(fs, data);
                }
            });
        }
    }
}