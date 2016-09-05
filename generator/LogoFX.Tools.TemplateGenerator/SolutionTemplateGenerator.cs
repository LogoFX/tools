﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using Project = Microsoft.Build.Evaluation.Project;

namespace LogoFX.Tools.TemplateGenerator
{
    public sealed class SolutionTemplateGenerator : GeneratorBase
    {
        private const string DefinitionsFolderName = "Definitions";
        private const string DefinitionTemplateName = "CSharp.vstemplate";

        private SolutionTemplateInfo _solutionTemplateInfo;

        private readonly string _solutionFileName;
        private string _currentName;

        public SolutionTemplateGenerator(string solutionFileName)
        {
            _solutionFileName = solutionFileName;
            _currentName = Path.GetFileNameWithoutExtension(_solutionFileName);
        }

        public async Task<ISolutionTemplateInfo> GetInfoAsync()
        {
            if (_solutionTemplateInfo == null)
            {
                _solutionTemplateInfo = await GenerateTemplateInfoAsync();
            }

            return _solutionTemplateInfo;
        }

        public async Task GenerateAsync(
            TemplateDataInfo templateData, 
            string destinationFolder, 
            ISolutionTemplateInfo solutionTemplateInfo,
            WizardConfiguration wizardConfiguration)
        {
            if (solutionTemplateInfo == null)
            {
                solutionTemplateInfo = await GetInfoAsync();
            }

            CleanDestination(destinationFolder, wizardConfiguration);

            if (!Directory.Exists(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);
            }

            CreateDefinitions(templateData, destinationFolder, solutionTemplateInfo);
            CreatePrepropcess(destinationFolder);

            var solutionFolder = destinationFolder;
            if (wizardConfiguration != null)
            {
                solutionFolder = Path.Combine(destinationFolder, _currentName);
                if (!Directory.Exists(solutionFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }
            }

            foreach (var projectTemplateInfo in solutionTemplateInfo.GetProjectsPlain())
            {
                var projectGenerator = new ProjectTemplateGenerator(projectTemplateInfo, solutionTemplateInfo);
                await projectGenerator.GenerateAsync(solutionFolder);
            }
        }

        private void CreateXElement(XElement rootElement, ISolutionFolderTemplateInfo solutionFolder)
        {
            foreach (var solutionItem in solutionFolder.Items)
            {
                if (solutionItem is ISolutionFolderTemplateInfo)
                {
                    var folderElement = new XElement(Ns + "SolutionFolder",
                        new XAttribute("Name", solutionItem.Name),
                        new XAttribute("CreateOnDisk", false));
                    rootElement.Add(folderElement);
                    CreateXElement(folderElement, (ISolutionFolderTemplateInfo) solutionItem);
                }
                else
                {
                    var projectTemplateInfo = (IProjectTemplateInfo) solutionItem;
                    var projectLinkElement = new XElement(Ns + "ProjectTemplateLink",
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

        private void CreateDefinitions(TemplateDataInfo templateData, string destinationFolder, ISolutionTemplateInfo solutionTemplateInfo)
        {
            var projectCollection = new XElement(Ns + "ProjectCollection");
            CreateXElement(projectCollection, solutionTemplateInfo);

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
                        "LogoFX.Tools.Templates.Wizard.SolutionWizard"),
                    MakeWizardExtension(
                        "TemplateBuilder, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null",
                        "TemplateBuilder.SolutionWizard")
                    ));

            XmlWriterSettings settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true
            };

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

        private void CleanDestination(string destinationFolder, WizardConfiguration wizardConfiguration)
        {
            if (!Directory.Exists(destinationFolder))
            {
                return;
            }

            if (wizardConfiguration == null)
            {
                Directory.Delete(destinationFolder, true);
                return;
            }

            var dir = new DirectoryInfo(destinationFolder);
            foreach (var info in dir.EnumerateFileSystemInfos())
            {
                if (info is FileInfo)
                {
                    info.Delete();
                    continue;
                }

                if (FileNameEquals(_currentName, info.Name) ||
                    wizardConfiguration.Solutions.All(x => !FileNameEquals(x.Name, info.Name)))
                {
                    ((DirectoryInfo) info).Delete(true);
                }
            }
        }

        private bool FileNameEquals(string fileName1, string filename2)
        {
            return string.Compare(fileName1, fileName1, StringComparison.OrdinalIgnoreCase) == 0;
        }

        private async Task<SolutionTemplateInfo> GenerateTemplateInfoAsync()
        {
            SolutionFile solution = SolutionFile.Parse(_solutionFileName);
            SolutionTemplateInfo solutionTemplateInfo = new SolutionTemplateInfo();

            var folders= new Dictionary<Guid, SolutionFolderTemplateInfo>();
            folders.Add(Guid.Empty, solutionTemplateInfo);

            foreach (var proj in solution.ProjectsInOrder)
            {
                await CreateSolutionItemTemplateInfoAsync(solution, solutionTemplateInfo, proj, folders);
            }

            ProjectCollection.GlobalProjectCollection.UnloadAllProjects();

            return solutionTemplateInfo;
        }

        private async Task<SolutionItemTemplateInfo> CreateSolutionItemTemplateInfoAsync(SolutionFile solution,
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
                    folder = (SolutionFolderTemplateInfo) await CreateSolutionItemTemplateInfoAsync(solution, solutionTemplateInfo, parentProj, folders);
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