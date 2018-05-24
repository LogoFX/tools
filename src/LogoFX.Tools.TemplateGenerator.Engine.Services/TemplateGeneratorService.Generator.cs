using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using LogoFX.Tools.TemplateGenerator.Data.Contracts.Dto;
using LogoFX.Tools.TemplateGenerator.Engine.Contracts;
using LogoFX.Tools.TemplateGenerator.Engine.Shared;
using Microsoft.Build.Evaluation;

namespace LogoFX.Tools.TemplateGenerator.Engine.Services
{
    internal sealed partial class TemplateGeneratorService
    {
        private async Task GenerateInternalAsync(SolutionConfigurationDto solutionConfiguration, Guid engineId, IProgress<double> progress)
        {
            var destinationFolder = solutionConfiguration.TemplateFolder;
            var engine = GetOrCreateEngine(engineId);

            await CleanDestination(destinationFolder);
            await CreateDefinitions(destinationFolder, engine.CreateDefinitionDocument(solutionConfiguration));
            await CreatePrepropcess(destinationFolder, engine.CreatePreprocessDocument(solutionConfiguration));

            var projects = await GetProjects(solutionConfiguration, engine);

            foreach (var projectInfo in projects)
            {
                var folderName = Path.GetDirectoryName(projectInfo.FileName);
                folderName = Path.GetFileName(folderName);
                var destinationFileName = CreateNewFileName(folderName, solutionConfiguration.Name, destinationFolder);
                projectInfo.DestinationFileName = destinationFileName;
                if (File.Exists(destinationFileName))
                {
                    continue;
                }

                await CopyProjectToTemplateAsync(projectInfo, engine, projects);
            }
        }

        private async Task CopyProjectToTemplateAsync(ProjectTemplateInfo projectInfo, ITemplateGeneratorEngine engine, ProjectTemplateInfo[] projects)
        {
            var projectFolder = Path.GetDirectoryName(projectInfo.DestinationFileName);
            Debug.Assert(projectFolder != null, "projectFolder != null");
            Directory.CreateDirectory(projectFolder);

            var from = Path.GetDirectoryName(projectInfo.FileName);

            if (File.Exists(projectInfo.DestinationFileName))
            {
                return;
            }

            File.Copy(projectInfo.FileName, projectInfo.DestinationFileName);

            Project project = new Project(projectInfo.DestinationFileName);

            var x = project.GetProperty("ProjectGuid");
            if (x != null)
            {
                x.UnevaluatedValue = "{$guid1$}";
            }

            x = project.GetProperty("RootNamespace");
            var rootNamespace = x.EvaluatedValue;
            if (!x.UnevaluatedValue.StartsWith("$("))
            {
                x.UnevaluatedValue = "$safeprojectname$";
            }

            x = project.GetProperty("AssemblyName");
            x.UnevaluatedValue = "$safeprojectname$";

            foreach (var item in project.Items.ToList())
            {
                string newFileName = null;

                switch (item.ItemType)
                {
                    case "ProjectReference":
                        FixReference(item, engine);
                        break;
                    case "Compile":
                    case "None":
                    case "Page":
                    case "ApplicationDefinition":
                    case "EmbeddedResource":
                    case "Content":
                    case "AppxManifest":
                    case "Service":
                    case "SDKReference":
                    case "Resource":
                        newFileName = await CopyProjectItem(item, from, projectFolder);
                        break;
                }

                if (string.IsNullOrWhiteSpace(newFileName))
                {
                    continue;
                }

                await engine.ProcessFileAsync(newFileName, rootNamespace, projects);
            }

            project.Save();
        }

        private async Task<string> CopyProjectItem(ProjectItem item, string from, string to)
        {
            var oldFileName = Path.Combine(from, item.EvaluatedInclude);

            if (!File.Exists(oldFileName))
            {
                return null;
            }

            var newFileName = Path.Combine(to, item.EvaluatedInclude);

            if (File.Exists(newFileName))
            {
                return newFileName;
            }

            var newFolder = Path.GetDirectoryName(newFileName);
            Debug.Assert(newFolder != null, "newFolder != null");
            if (!Directory.Exists(newFolder))
            {
                Directory.CreateDirectory(newFolder);
            }

            await Task.Run(() => { File.Copy(oldFileName, newFileName); });

            return newFileName;
        }

        private void FixReference(ProjectItem reference, ITemplateGeneratorEngine engine)
        {
            var include = reference.EvaluatedInclude;
            var fileName = Path.GetFileName(include);
            var dir = Path.GetFileNameWithoutExtension(fileName);
            var ddir = Path.GetDirectoryName(Path.GetDirectoryName(include));
            Debug.Assert(ddir != null, nameof(ddir) + " != null");
            include = Path.Combine(ddir, Path.Combine(dir, fileName));
            var rootName = engine.GetRootName(include);
            reference.UnevaluatedInclude = include.Replace(rootName, "$saferootprojectname$");
            var name = reference.Metadata.SingleOrDefault(x => x.Name == "Name");
            if (name != null)
            {
                name.UnevaluatedValue = name.EvaluatedValue.Replace(rootName, "$saferootprojectname$");
            }
        }

        private string CreateNewFileName(string projectName, string solutionName, string destinationFolder)
        {
            var solutionFolder = Path.Combine(destinationFolder, solutionName);

            var newProjectName = projectName;
            Debug.Assert(newProjectName != null, "newProjectName != null");
            if (newProjectName.Length > 12)
            {
                newProjectName = "MyProject.csproj";
            }

            var result = Path.Combine(solutionFolder, projectName);
            result = Path.Combine(result, newProjectName);

            return result;
        }

        private Task CreatePrepropcess(string destinationFolder, XDocument preprocessDocument)
        {
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true
            };

            var definitionFile = Path.Combine(destinationFolder, "_preprocess.xml");

            return Task.Run(() =>
            {
                using (XmlWriter xw = XmlWriter.Create(definitionFile, settings))
                {
                    preprocessDocument.Save(xw);
                }
            });
        }

        private Task CreateDefinitions(string destinationFolder, XDocument definitionDocument)
        {
            XmlWriterSettings settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true
            };

            string GetDefinitionFileName()
            {
                const string definitionsFolderName = "Definitions";
                const string definitionTemplateName = "CSharp.vstemplate";

                var definitionsPath = Path.Combine(destinationFolder, definitionsFolderName);
                Directory.CreateDirectory(definitionsPath);
                var definitionFile = Path.Combine(definitionsPath, definitionTemplateName);
                return definitionFile;
            }

            return Task.Run(() =>
            {
                var definitionFile = GetDefinitionFileName();

                using (XmlWriter xw = XmlWriter.Create(definitionFile, settings))
                {
                    definitionDocument.Save(xw);
                }
            });
        }

        private Task CleanDestination(string destinationFolder)
        {
            return Task.Run(() =>
            {
                if (!Directory.Exists(destinationFolder))
                {
                    return;
                }

                var dirInfo = new DirectoryInfo(destinationFolder);
                foreach (var subDir in dirInfo.EnumerateDirectories())
                {
                    subDir.Delete(true);
                }
            });
        }

        private async Task<ProjectTemplateInfo[]> GetProjects(SolutionConfigurationDto solutionConfiguration, ITemplateGeneratorEngine engine)
        {
            var result = await CreateProjectsAsync(solutionConfiguration, engine);
            
            ProjectCollection.GlobalProjectCollection.UnloadAllProjects();
            
            return result;
        }

        private async Task<ProjectTemplateInfo[]> CreateProjectsAsync(SolutionConfigurationDto solutionConfiguration, ITemplateGeneratorEngine engine)
        {
            var solutionTemplateInfo = await engine.CreateSolutionInfoAsync(solutionConfiguration);
            return GetPlainProjects(solutionTemplateInfo.Items).ToArray();
        }

        private IEnumerable<ProjectTemplateInfo> GetPlainProjects(IEnumerable<SolutionItemTemplateInfo> items)
        {
            var result = new List<ProjectTemplateInfo>();

            foreach (var item in items)
            {
                if (item is SolutionFolderTemplateInfo folder)
                {
                    result.AddRange(GetPlainProjects(folder.Items));
                    continue;
                }

                if (item is ProjectTemplateInfo project)
                {
                    result.Add(project);
                    continue;
                }

                throw new ArgumentException($"Unknown Solution Item type '{item.GetType().Name}'.");
            }

            return result;
        }
    }
}