using System.IO;
using System.Xml.Serialization;

namespace LogoFX.Tools.Common.Model
{
    public static class WizardDataLoader
    {
        public static WizardData Load(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return null;
            }

            var serializer = new XmlSerializer(typeof(WizardData));
            using (var fs = File.OpenRead(fileName))
            {
                return (WizardData) serializer.Deserialize(fs);
            }
        }

        public static void Save(string fileName, WizardData data)
        {
            var serializer = new XmlSerializer(typeof(WizardData));
            using (var fs = File.Create(fileName))
            {
                serializer.Serialize(fs, data);
            }
        }
    }
}