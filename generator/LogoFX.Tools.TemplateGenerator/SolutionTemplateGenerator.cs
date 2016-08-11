using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;

namespace LogoFX.Tools.TemplateGenerator
{
    public sealed class SolutionTemplateGenerator : GeneratorBase
    {
        private const string DefinitionsFolderName = "Definitions";
        private const string DefinitionTemplateName = "CSharp.vstemplate";

        private SolutionTemplateInfo _solutionTemplateInfo;

        private readonly string _solutioFileName;
        private readonly TemplateDataInfo _templateData;

        public SolutionTemplateGenerator(string solutioFileName, TemplateDataInfo templateData)
        {
            _solutioFileName = solutioFileName;
            _templateData = templateData;
        }

        public ISolutionTemplateInfo GetInfo()
        {
            return _solutionTemplateInfo ?? (_solutionTemplateInfo = GenerateTemplateInfo());
        }

        public async Task GenerateAsync(string destinationFolder, ISolutionTemplateInfo solutionTemplateInfo = null)
        {
            if (solutionTemplateInfo == null)
            {
                solutionTemplateInfo = GetInfo();
            }

            CleanDestination(destinationFolder);
            Directory.CreateDirectory(destinationFolder);

            CreateDefinitions(destinationFolder, solutionTemplateInfo);
            CreatePrepropcess(destinationFolder);

            foreach (var projectTemplateInfo in solutionTemplateInfo.GetProjectsPlain())
            {
                var projectGenerator = new ProjectTemplateGenerator(projectTemplateInfo, solutionTemplateInfo);
                await projectGenerator.GenerateAsync(destinationFolder);
            }
        }

        private void CreateXElement(XElement rootElement, ISolutionFolderTemplateInfo solutionFolder)
        {
            foreach (var solutionItem in solutionFolder.Items)
            {
                if (solutionItem is ISolutionFolderTemplateInfo)
                {
                    var folderElement = new XElement(s_ns + "SolutionFolder",
                        new XAttribute("Name", solutionItem.Name),
                        new XAttribute("CreateOnDisk", false));
                    rootElement.Add(folderElement);
                    CreateXElement(folderElement, (ISolutionFolderTemplateInfo) solutionItem);
                }
                else
                {
                    var projectTemplateInfo = (IProjectTemplateInfo) solutionItem;
                    var projectLinkElement = new XElement(s_ns + "ProjectTemplateLink",
                        new XAttribute("ProjectName", SafeProjectName(projectTemplateInfo)),
                        VSTemplateName(projectTemplateInfo));
                    rootElement.Add(projectLinkElement);
                }
            }
        }

        private string VSTemplateName(IProjectTemplateInfo projectTemplateInfo)
        {
            return $"{projectTemplateInfo.Name}\\MyTemplate.vstemplate";
        }

        private void CreateDefinitions(string destinationFolder, ISolutionTemplateInfo solutionTemplateInfo)
        {
            var projectCollection = new XElement(s_ns + "ProjectCollection");
            CreateXElement(projectCollection, solutionTemplateInfo);

            var doc = new XDocument(
                new XElement(s_ns + "VSTemplate",
                    new XAttribute("Version", "3.0.0"),
                    new XAttribute("Type", "ProjectGroup"),
                    new XElement(s_ns + "TemplateData",
                        new XElement(s_ns + "Name", _templateData.Name),
                        new XElement(s_ns + "Description", _templateData.Description),
                        new XElement(s_ns + "ProjectType", _templateData.ProjectType),
                        new XElement(s_ns + "DefaultName", _templateData.DefaultName),
                        new XElement(s_ns + "SortOrder", _templateData.SortOrder),
                        new XElement(s_ns + "Icon", _templateData.DefaultName)),
                    new XElement(s_ns + "TemplateContent",
                        projectCollection),
                    new XElement(s_ns + "WizardExtension",
                        new XElement(s_ns + "Assembly", "LogoFX.Tools.Templates.Wizard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"),
                        new XElement(s_ns + "FullClassName", "LogoFX.Tools.Templates.Wizard.SolutionWizard")
                        )));

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;

            var definitionsPath = Path.Combine(destinationFolder, DefinitionsFolderName);
            Directory.CreateDirectory(definitionsPath);
            var definitionFile = Path.Combine(definitionsPath, DefinitionTemplateName);

            using (XmlWriter xw = XmlWriter.Create(definitionFile, settings))
            {
                doc.Save(xw);
            }
        }

        private void CreatePrepropcess(string destinationFolder)
        {
            var doc = new XDocument(
                new XElement("Preprocess",
                    new XElement("TemplateInfo",
                        new XAttribute("Path", "CSharp\\LogoFX")),
                    new XElement("Replacements",
                        new XAttribute("Include", "*.*"),
                        new XAttribute("Exclude", "*.vstemplate;*.csproj;*.fsproj;*.vbproj;*.jpg;*.png;*.ico;_preprocess.xml;_project.vstemplate.xml"))));

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            var definitionFile = Path.Combine(destinationFolder, "_preprocess.xml");

            using (XmlWriter xw = XmlWriter.Create(definitionFile, settings))
            {
                doc.Save(xw);
            }
        }

        private void CleanDestination(string destinationFolder)
        {
            if (!Directory.Exists(destinationFolder))
            {
                return;
            }

            Directory.Delete(destinationFolder, true);
        }

        private SolutionTemplateInfo GenerateTemplateInfo()
        {
            SolutionFile solution = SolutionFile.Parse(_solutioFileName);
            SolutionTemplateInfo solutionTemplateInfo = new SolutionTemplateInfo();

            var folders= new Dictionary<Guid, SolutionFolderTemplateInfo>();
            folders.Add(Guid.Empty, solutionTemplateInfo);

            foreach (var proj in solution.ProjectsInOrder)
            {
                CreateSolutionItemTemplateInfo(solution, solutionTemplateInfo, proj, folders);
            }

            ProjectCollection.GlobalProjectCollection.UnloadAllProjects();

            return solutionTemplateInfo;
        }

        private SolutionItemTemplateInfo CreateSolutionItemTemplateInfo(SolutionFile solution,
            SolutionTemplateInfo solutionTemplateInfo,
            ProjectInSolution proj, 
            IDictionary<Guid, SolutionFolderTemplateInfo> folders)
        {
            var id = Guid.Parse(proj.ProjectGuid);
            var parentId = proj.ParentProjectGuid == null ? Guid.Empty : Guid.Parse(proj.ParentProjectGuid);

            SolutionFolderTemplateInfo folder;
            if (!folders.TryGetValue(parentId, out folder))
            {
                ProjectInSolution parentProj;
                if (solution.ProjectsByGuid.TryGetValue(proj.ParentProjectGuid, out parentProj))
                {
                    folder = (SolutionFolderTemplateInfo) CreateSolutionItemTemplateInfo(solution, solutionTemplateInfo, parentProj, folders);
                }
            }

            Debug.Assert(folder != null);

            SolutionItemTemplateInfo result;

            switch (proj.ProjectType)
            {
                case SolutionProjectType.KnownToBeMSBuildFormat:
                    Project project = new Project(proj.AbsolutePath);
                    var rootNamespace = project.Properties.Single(x => x.Name == "RootNamespace").EvaluatedValue;
                    var rootName = AddRootName(rootNamespace, solutionTemplateInfo);
                    result = new ProjectTemplateInfo(id, proj.ProjectName)
                    {
                        NameWithoutRoot = proj.ProjectName.Substring(rootName.Length + 1),
                        FileName = proj.AbsolutePath
                    };
                    break;
                case SolutionProjectType.SolutionFolder:
                    if (folders.ContainsKey(id))
                    {
                        return folders[id];
                    }
                    result = new SolutionFolderTemplateInfo(id, proj.ProjectName);
                    folders.Add(id, (SolutionFolderTemplateInfo) result);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            folder.Items.Add(result);

            return result;
        }

        private string AddRootName(string projectName, SolutionTemplateInfo solutionTemplateInfo)
        {
            var rootName = GetRootName(projectName);

            if (!solutionTemplateInfo.RootNamespaces.Contains(rootName))
            {
                solutionTemplateInfo.RootNamespaces.Add(rootName);
            }

            return rootName;
        }
    }
}