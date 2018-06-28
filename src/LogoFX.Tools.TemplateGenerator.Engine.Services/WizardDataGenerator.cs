using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LogoFX.Tools.Common.Model;
using LogoFX.Tools.TemplateGenerator.Engine.Shared;

namespace LogoFX.Tools.TemplateGenerator.Engine.Services
{
    public sealed class WizardDataGenerator
    {
        public async Task GenerateAndSaveAsync(SolutionTemplateInfo solutionInfo, string destinationFolder)
        {
            var wizardData = CreateWizardData(solutionInfo, destinationFolder);
            var fileName = Path.Combine(destinationFolder, WizardDataLoader.WizardDataFielName);
            await WizardDataLoader.SaveAsync(fileName, wizardData);
        }

        private WizardData CreateWizardData(SolutionTemplateInfo solutionInfo, string destinationFolder)
        {
            WizardData wizardData = new WizardData
            {
                Title = $"New {solutionInfo.Description}",
                Solution = CreateSolutionData(solutionInfo, destinationFolder)
            };

            return wizardData;
        }

        private SolutionData CreateSolutionData(SolutionTemplateInfo solutionInfo, string destinationFolder)
        {
            SolutionData solutionData = new SolutionData
            {
                Name = solutionInfo.Name,
                Caption = solutionInfo.Name,
                IconFileName = solutionInfo.IconPath,
                PostCreateUrl = solutionInfo.PostCreateUrl,
                Items = solutionInfo.Items.Select(x => CreateSolutionItemData(x, destinationFolder)).ToArray()
            };

            return solutionData;
        }

        private SolutionItemData CreateSolutionItemData(SolutionItemTemplateInfo solutionItemInfo, string destinationFolder)
        {
            SolutionItemData solutionItemData;

            if (solutionItemInfo is SolutionFolderTemplateInfo solutionFolderInfo)
            {
                solutionItemData = CreateSolutionFolderData(solutionFolderInfo, destinationFolder);
                solutionItemData.Name = solutionItemInfo.Name;
                return solutionItemData;
            }

            if (solutionItemInfo is ProjectTemplateInfo projectInfo)
            {
                solutionItemData = CreateProjectData(projectInfo, destinationFolder);
                return solutionItemData;
            }

            throw new ArgumentException("Unknown solution item type.", nameof(solutionItemInfo));
        }

        private SolutionFolderData CreateSolutionFolderData(SolutionFolderTemplateInfo solutionFolderInfo, string destinationFolder)
        {
            SolutionFolderData solutionFolderData = new SolutionFolderData
            {
                Items = solutionFolderInfo.Items
                    .Select(info => CreateSolutionItemData(info, destinationFolder))
                    .ToArray()
            };
            return solutionFolderData;
        }

        private static string GetRelativePath(string filespec, string folder)
        {
            if (filespec == null)
            {
                Debugger.Break();
            }

            Debug.Assert(filespec != null, nameof(filespec) + " != null");

            var pathUri = new Uri(filespec);
            
            // Folders must end in a slash
            if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                folder += Path.DirectorySeparatorChar;
            }

            var folderUri = new Uri(folder);
            return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
        }

        private ProjectData CreateProjectData(ProjectTemplateInfo projectInfo, string destinationFolder)
        {
            ProjectData projectData = new ProjectData
            {
                Name = projectInfo.NameWithoutRoot,
                IsStartup = projectInfo.IsStartup,
                FileName = GetRelativePath(projectInfo.DestinationFileName, destinationFolder),
                ProjectConfigurations = CreateProjectConfigurations(projectInfo.ProjectConfigurations)
            };


            return projectData;
        }

        private ProjectConfigurationData[] CreateProjectConfigurations(IEnumerable<ProjectConfiguration> projectConfigurations)
        {
            var result = new List<ProjectConfigurationData>();
            foreach (var projectConfiguration in projectConfigurations)
            {
                result.Add(new ProjectConfigurationData
                {
                    ConfigurationName = projectConfiguration.ConfigurationName,
                    Name = projectConfiguration.Name,
                    IncludeInBuild = projectConfiguration.IncludeInBuild,
                    PlatformName = projectConfiguration.PlatformName
                });
            }
            return result.ToArray();
        }
    }
}