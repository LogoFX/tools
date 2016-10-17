using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace LogoFX.Tools.TemplateGenerator
{
    internal sealed class DefinitionsGenerator : GeneratorBase
    {
        private const string DefinitionsFolderName = "Definitions";
        private const string DefinitionTemplateName = "CSharp.vstemplate";

        private readonly string _destinationFolder;

        public DefinitionsGenerator(string destinationFolder)
        {
            _destinationFolder = destinationFolder;
        }

        public void CreateDefinitions(WizardConfiguration wizardConfiguration)
        {
            var projectCollection = new XElement(Ns + "ProjectCollection");

            var doc = new XDocument(
                new XElement(Ns + "VSTemplate",
                    new XAttribute("Version", "3.0.0"),
                    new XAttribute("Type", "ProjectGroup"),
                    new XElement(Ns + "TemplateData",
                        new XElement(Ns + "Name", wizardConfiguration.Name),
                        new XElement(Ns + "Description", wizardConfiguration.Description),
                        new XElement(Ns + "ProjectType", wizardConfiguration.ProjectType),
                        new XElement(Ns + "DefaultName", wizardConfiguration.DefaultName),
                        new XElement(Ns + "SortOrder", wizardConfiguration.SortOrder),
                        new XElement(Ns + "Icon", wizardConfiguration.IconName)),
                    new XElement(Ns + "TemplateContent", projectCollection),
                    MakeWizardExtension(
                        "LogoFX.Tools.Templates.Wizard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
                        $"LogoFX.Tools.Templates.Wizard.SolutionWizard")
                    ));

            XmlWriterSettings settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true
            };

            var definitionFile = GetDefinitionFileName();

            using (XmlWriter xw = XmlWriter.Create(definitionFile, settings))
            {
                doc.Save(xw);
            }
        }

        private string GetDefinitionFileName()
        {
            var definitionsPath = Path.Combine(_destinationFolder, DefinitionsFolderName);
            Directory.CreateDirectory(definitionsPath);
            var definitionFile = Path.Combine(definitionsPath, DefinitionTemplateName);
            return definitionFile;
        }
    }
}