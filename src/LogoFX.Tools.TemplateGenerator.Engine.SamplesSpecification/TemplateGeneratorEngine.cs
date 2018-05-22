using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LogoFX.Tools.TemplateGenerator.Engine.Contracts;
using LogoFX.Tools.TemplateGenerator.Engine.Shared;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;

namespace LogoFX.Tools.TemplateGenerator.Engine.SamplesSpecification
{
    [UsedImplicitly]
    internal sealed class TemplateGeneratorEngine : ITemplateGeneratorEngine
    {
        private static readonly string[] _rootNamespaces =
        {
            "Samples.Specifications.",
            "Samples."
        };

        public string Name => "Samples.Specifications";

        public async Task<SolutionTemplateInfo> CreateSolutionInfoAsync(string solutionFileName)
        {
            SolutionFile solution = SolutionFile.Parse(solutionFileName);

            var folders = new Dictionary<Guid, SolutionFolderTemplateInfo>();
            var solutionTemplateInfo = new SolutionTemplateInfo();
            folders.Add(Guid.Empty, solutionTemplateInfo);

            foreach (var proj in solution.ProjectsInOrder)
            {
                await CreateSolutionItemTemplateInfoAsync(solution, solutionTemplateInfo, proj, folders);
            }

            return solutionTemplateInfo;
        }

        private async Task<SolutionItemTemplateInfo> CreateSolutionItemTemplateInfoAsync(
            SolutionFile solution,
            SolutionTemplateInfo solutionTemplateInfo,
            ProjectInSolution proj,
            IDictionary<Guid, SolutionFolderTemplateInfo> folders)
        {
            var id = Guid.Parse(proj.ProjectGuid);
            var parentId = proj.ParentProjectGuid == null ? Guid.Empty : Guid.Parse(proj.ParentProjectGuid);

            if (!folders.TryGetValue(parentId, out var folder))
            {
                Debug.Assert(proj.ParentProjectGuid != null, "proj.ParentProjectGuid != null");
                if (solution.ProjectsByGuid.TryGetValue(proj.ParentProjectGuid, out var parentProj))
                {
                    folder = (SolutionFolderTemplateInfo)await CreateSolutionItemTemplateInfoAsync(solution, solutionTemplateInfo, parentProj, folders);
                }
            }

            Debug.Assert(folder != null);

            SolutionItemTemplateInfo result;

            switch (proj.ProjectType)
            {
                case SolutionProjectType.KnownToBeMSBuildFormat:
                    var project = new Project(proj.AbsolutePath);
                    var rootNamespace = project.Properties.Single(x => x.Name == "RootNamespace").EvaluatedValue;
                    var targetName = project.Properties.Single(x => x.Name == "TargetName").EvaluatedValue;
                    var rootName = AddRootName(rootNamespace, solutionTemplateInfo);
                    result = new ProjectTemplateInfo(id, proj.ProjectName)
                    {
                        NameWithoutRoot = targetName.Substring(rootName.Length + 1),
                        FileName = proj.AbsolutePath,
                        //ProjectConfigurations = GetProjectConfigurations(proj.ProjectConfigurations)
                    };
                    break;
                case SolutionProjectType.SolutionFolder:
                    if (folders.ContainsKey(id))
                    {
                        return folders[id];
                    }
                    result = new SolutionFolderTemplateInfo(id, proj.ProjectName);
                    folders.Add(id, (SolutionFolderTemplateInfo)result);
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

        private string GetRootName(string projectName)
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
    }
}