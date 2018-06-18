using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using LogoFX.Tools.TemplateGenerator.Data.Contracts.Dto;
using LogoFX.Tools.TemplateGenerator.Engine.Shared;
using Microsoft.Build.Construction;

namespace LogoFX.Tools.TemplateGenerator.Engine.SamplesSpecification
{
    internal sealed partial class TemplateGeneratorEngine
    {
        public string Name => "Samples.Specifications";

        public async Task<SolutionTemplateInfo> CreateSolutionInfoAsync(SolutionConfigurationDto solutionConfiguration)
        {
            SolutionFile solution = SolutionFile.Parse(solutionConfiguration.Path);

            var folders = new Dictionary<Guid, SolutionFolderTemplateInfo>();
            var solutionTemplateInfo = new SolutionTemplateInfo();
            folders.Add(Guid.Empty, solutionTemplateInfo);

            foreach (var proj in solution.ProjectsInOrder)
            {
                await CreateSolutionItemTemplateInfoAsync(solution, proj, folders);
            }

            return solutionTemplateInfo;
        }

        public XDocument CreateDefinitionDocument(SolutionConfigurationDto solutionConfiguration)
        {
            return CreateDefinitionInternal(solutionConfiguration);
        }

        public string GetRootName(string projectName)
        {
            projectName = Path.GetFileName(projectName);

            Debug.Assert(projectName != null, nameof(projectName) + " != null");

            string rootName = null;
            foreach (var rootNamespace in _rootNamespaces)
            {
                if (projectName.StartsWith(rootNamespace))
                {
                    rootName = rootNamespace;
                }
            }

            return rootName;
        }

        public string[] GetRootNamespaces()
        {
            return _rootNamespaces;
        }

        // ReSharper disable once RedundantAssignment
        public string CreateNewFileName(string projectName, string solutionName, string destinationFolder, int index)
        {
            var solutionFolder = Path.Combine(destinationFolder, solutionName);
            projectName = "P" + index;
            var result = Path.Combine(solutionFolder, projectName);
            result = Path.Combine(result, projectName + ".csproj");

            return result;
        }

        public Task ProcessFileAsync(string fileName, string rootNamespace, ProjectTemplateInfo[] projects)
        {
            var ext = Path.GetExtension(fileName);
            switch (ext)
            {
                case ".cs":
                case ".config":
                    return ProcessCs(fileName, rootNamespace, projects);
                case ".xaml":
                    return ProcessXaml(fileName, rootNamespace, projects);
            }

            return Task.CompletedTask;
        }
    }
}