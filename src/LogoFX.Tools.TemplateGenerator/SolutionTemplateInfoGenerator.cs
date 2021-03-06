﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LogoFX.Tools.TemplateGenerator.Contracts;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;

namespace LogoFX.Tools.TemplateGenerator
{
    public sealed class SolutionTemplateInfoGenerator : GeneratorBase
    {
        public async Task<ISolutionTemplateInfo> GenerateTemplateInfoAsync(string solutionFileName)
        {
            SolutionTemplateInfo solutionTemplateInfo = new SolutionTemplateInfo();

            SolutionFile solution = SolutionFile.Parse(solutionFileName);

            await CreateProjectsAsync(solution, solutionTemplateInfo);

            ProjectCollection.GlobalProjectCollection.UnloadAllProjects();

            return solutionTemplateInfo;
        }

        private async Task CreateProjectsAsync(SolutionFile solution, SolutionTemplateInfo solutionTemplateInfo)
        {
            var folders = new Dictionary<Guid, SolutionFolderTemplateInfo>();
            folders.Add(Guid.Empty, solutionTemplateInfo);

            foreach (var proj in solution.ProjectsInOrder)
            {
                await CreateSolutionItemTemplateInfoAsync(solution, solutionTemplateInfo, proj, folders);
            }
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
                    Project project = new Project(proj.AbsolutePath);
                    var rootNamespace = project.Properties.Single(x => x.Name == "RootNamespace").EvaluatedValue;
                    var targetName = project.Properties.Single(x => x.Name == "TargetName").EvaluatedValue;
                    var rootName = AddRootName(rootNamespace, solutionTemplateInfo);
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