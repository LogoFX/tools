using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    internal sealed partial class TemplateGeneratorEngine : ITemplateGeneratorEngine
    {
        private static readonly string[] _rootNamespaces =
        {
            "Samples.Specifications.",
            "Samples."
        };

        private async Task<SolutionItemTemplateInfo> CreateSolutionItemTemplateInfoAsync(
            SolutionFile solution,
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
                    folder = (SolutionFolderTemplateInfo) await CreateSolutionItemTemplateInfoAsync(solution, parentProj, folders);
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
                    var rootName = GetRootName(rootNamespace);
                    result = new ProjectTemplateInfo(id, proj.ProjectName)
                    {
                        NameWithoutRoot = targetName.Substring(rootName.Length + 1),
                        FileName = proj.AbsolutePath,
                        ProjectConfigurations = GetProjectConfigurations(proj.ProjectConfigurations)
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

        private ProjectConfiguration[] GetProjectConfigurations(IEnumerable<KeyValuePair<string,  ProjectConfigurationInSolution>> projectConfigurations)
        {
            var result = new List<ProjectConfiguration>();

            foreach (var pair in projectConfigurations)
            {
                result.Add(new ProjectConfiguration
                {
                    Name = pair.Key,
                    ConfigurationName = pair.Value.ConfigurationName,
                    PlatformName = pair.Value.PlatformName,
                    IncludeInBuild = pair.Value.IncludeInBuild
                });
            }

            return result.ToArray();
        }
    }
}