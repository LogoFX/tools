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

            foreach (var projectTemplateInfo in projects)
            {
                var folderName = Path.GetDirectoryName(projectTemplateInfo.FileName);
                folderName = Path.GetFileName(folderName);
                var destinationFileName = CreateNewFileName(folderName, solutionConfiguration.Name, destinationFolder);
                projectTemplateInfo.DestinationFileName = destinationFileName;
                if (File.Exists(destinationFileName))
                {
                    continue;
                }
                //var projectGenerator = new ProjectTemplateGenerator(projectTemplateInfo, variant.SolutionTemplateInfo.RootNamespaces, projects);
                //await projectGenerator.GenerateAsync();
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