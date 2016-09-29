using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly string _subFolder;

        public DefinitionsGenerator(string destinationFolder, string subFolder)
        {
            _destinationFolder = destinationFolder;
            _subFolder = subFolder;
        }

        public ISolutionFolderTemplateInfo LoadDefinitions()
        {
            var definitionFile = GetDefinitionFileName();

            if (!File.Exists(definitionFile))
            {
                return null;
            }

            XDocument doc;
            using (var fs = File.OpenRead(definitionFile))
            {
                doc = XDocument.Load(fs);
            }

            var solutionTemplateInfo = new SolutionTemplateInfo();
            var projectCollection = GetProjectCollection(doc);
            foreach (var item in projectCollection)
            {
                solutionTemplateInfo.Items.Add(item);
            }
            return solutionTemplateInfo;
        }

        private XElement GetContentTemplateElement(XDocument doc)
        {
            var vsTemplateElement = doc.Element(Ns + "VSTemplate");
            var templateContentElement = vsTemplateElement.Element(Ns + "TemplateContent");

            return templateContentElement;
        }

        private XElement GetProjectCollectionElement(XElement templateContentElement)
        {
            var result = templateContentElement.Element(Ns + "ProjectCollection");
            return result;
        }

        private IEnumerable<SolutionItemTemplateInfo> GetProjectCollection(XDocument doc)
        {
            var templateContentElement = GetContentTemplateElement(doc);
            var projectCollectionElement = GetProjectCollectionElement(templateContentElement);
            return GetSolutionItems(projectCollectionElement);
        }

        private IEnumerable<SolutionItemTemplateInfo> GetSolutionItems(XElement element)
        {
            foreach (var e in element.Elements())
            {
                switch (e.Name.LocalName)
                {
                    case "SolutionFolder":
                        yield return GetSolutionFolder(e);
                        break;
                    case "ProjectTemplateLink":
                        yield return GetProject(e);
                        break;
                    default:
                        throw new ArgumentException($"Unknown XML elemnt '{element.Name.LocalName}'");
                }
            }
        }

        private SolutionFolderTemplateInfo GetSolutionFolder(XElement folderElement)
        {
            var name = folderElement.Attribute("Name").Value;
            SolutionFolderTemplateInfo result = new SolutionFolderTemplateInfo(Guid.Empty, name);
            foreach (var item in GetSolutionItems(folderElement))
            {
                result.Items.Add(item);
            }
            return result;
        }

        private ProjectTemplateInfo GetProject(XElement projectElement)
        {
            var name = projectElement.Attribute("ProjectName").Value;
            ProjectTemplateInfo result = new ProjectTemplateInfo(Guid.Empty, name);
            result.FileName = projectElement.Value;
            result.NameWithoutRoot = name.Substring(name.LastIndexOf("$.", StringComparison.Ordinal) + 2);
            return result;
        }

        public void CreateDefinitions(TemplateDataInfo templateData, ISolutionTemplateInfo solutionTemplateInfo)
        {
            var projectCollection = new XElement(Ns + "ProjectCollection");
            CreateItems(projectCollection, solutionTemplateInfo);

            var doc = new XDocument(
                new XElement(Ns + "VSTemplate",
                    new XAttribute("Version", "3.0.0"),
                    new XAttribute("Type", "ProjectGroup"),
                    new XElement(Ns + "TemplateData",
                        new XElement(Ns + "Name", templateData.Name),
                        new XElement(Ns + "Description", templateData.Description),
                        new XElement(Ns + "ProjectType", templateData.ProjectType),
                        new XElement(Ns + "DefaultName", templateData.DefaultName),
                        new XElement(Ns + "SortOrder", templateData.SortOrder),
                        new XElement(Ns + "Icon", templateData.DefaultName)),
                    new XElement(Ns + "TemplateContent", projectCollection),
                    MakeWizardExtension(
                        "LogoFX.Tools.Templates.Wizard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
                        $"LogoFX.Tools.Templates.Wizard.{templateData.WizardClassName}"),
                    MakeWizardExtension(
                        "TemplateBuilder, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null",
                        "TemplateBuilder.SolutionWizard")
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

        private void CreateItems(XElement rootElement, ISolutionFolderTemplateInfo solutionFolder)
        {
            foreach (var solutionItem in solutionFolder.Items)
            {
                if (solutionItem is ISolutionFolderTemplateInfo)
                {
                    var folderElement = new XElement(Ns + "SolutionFolder",
                        new XAttribute("Name", solutionItem.Name),
                        new XAttribute("CreateOnDisk", false));
                    rootElement.Add(folderElement);
                    CreateItems(folderElement, (ISolutionFolderTemplateInfo)solutionItem);
                }
                else
                {
                    var projectTemplateInfo = (IProjectTemplateInfo)solutionItem;
                    var projectLinkElement = new XElement(Ns + "ProjectTemplateLink",
                        new XAttribute("ProjectName", SafeProjectName(projectTemplateInfo)),
                        VSTemplateName(projectTemplateInfo));
                    rootElement.Add(projectLinkElement);
                }
            }
        }

        private string VSTemplateName(IProjectTemplateInfo projectTemplateInfo)
        {
            if (string.IsNullOrEmpty(_subFolder))
            {
                return $"{projectTemplateInfo.Name}\\MyTemplate.vstemplate";
            }

            return $"{_subFolder}\\{projectTemplateInfo.Name}\\MyTemplate.vstemplate";
        }
    }
}