using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using LogoFX.Tools.TemplateGenerator.Contracts;

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
            CreateItems(projectCollection, wizardConfiguration);

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
                        $"LogoFX.Tools.Templates.Wizard.{Path.GetFileNameWithoutExtension(wizardConfiguration.CodeFileName)}"),
                    MakeWizardExtension(
                        "TemplateBuilder, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null",
                        "TemplateBuilder.SolutionWizard"),
                    MakeWizardExtension(
                        "LogoFX.Tools.Templates.Wizard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
                        $"LogoFX.Tools.Templates.Wizard.PostSolutionWizard")
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

        private void CreateItems(XElement rootElement, WizardConfiguration wizardConfiguration)
        {
            if (wizardConfiguration.Solutions.Count == 1)
            {
                CreateItems(rootElement, wizardConfiguration.Solutions.First().SolutionTemplateInfo, null);
                return;
            }

            foreach (var solution in wizardConfiguration.Solutions)
            {
                var folderElement = new XElement(Ns + "SolutionFolder",
                    new XAttribute("Name", solution.Name),
                    new XAttribute("CreateOnDisk", true));
                rootElement.Add(folderElement);
                CreateItems(folderElement, solution.SolutionTemplateInfo, solution.Name);
            }
        }

        private void CreateItems(XElement rootElement, ISolutionFolderTemplateInfo solutionFolder, string subFolder)
        {
            foreach (var solutionItem in solutionFolder.Items)
            {
                var folder = solutionItem as ISolutionFolderTemplateInfo;
                if (folder != null)
                {
                    var folderElement = new XElement(Ns + "SolutionFolder",
                        new XAttribute("Name", folder.Name),
                        new XAttribute("CreateOnDisk", false));
                    rootElement.Add(folderElement);
                    CreateItems(folderElement, folder, subFolder);
                }
                else
                {
                    var projectTemplateInfo = (IProjectTemplateInfo)solutionItem;
                    var projectLinkElement = new XElement(Ns + "ProjectTemplateLink",
                        new XAttribute("ProjectName", SafeProjectName(projectTemplateInfo)),
                        VSTemplateName(projectTemplateInfo, subFolder));
                    rootElement.Add(projectLinkElement);
                }
            }
        }

        private string VSTemplateName(IProjectTemplateInfo projectTemplateInfo, string subFolder)
        {
            if (string.IsNullOrEmpty(subFolder))
            {
                return $"{projectTemplateInfo.Name}\\MyTemplate.vstemplate";
            }

            return $"{subFolder}\\{projectTemplateInfo.Name}\\MyTemplate.vstemplate";
        }
    }
}