﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LogoFX.Tools.Common.Model;
using LogoFX.Tools.TemplateGenerator.Contracts;
using Task = System.Threading.Tasks.Task;

namespace LogoFX.Tools.TemplateGenerator
{
    public sealed class WizardDataGenerator
    {
        private readonly WizardConfiguration _wizardConfiguration;

        public WizardDataGenerator(WizardConfiguration wizardConfiguration)
        {
            _wizardConfiguration = wizardConfiguration;
        }

        public async Task GenerateAndSaveAsync(string destinationFolder)
        {
            var wizardData = CreateWizardData(destinationFolder);
            var fileName = Path.Combine(destinationFolder, WizardDataLoader.WizardDataFielName);
            await WizardDataLoader.SaveAsync(fileName, wizardData);
        }

        private WizardData CreateWizardData(string destinationFolder)
        {
            WizardData wizardData = new WizardData
            {
                Title = $"New {_wizardConfiguration.Description}",
                Solutions = _wizardConfiguration.Solutions
                    .Select(info => CreateSolutionData(info, destinationFolder))
                    .ToArray()
            };
            return wizardData;
        }

        private SolutionData CreateSolutionData(SolutionInfo solutionInfo, string destinationFolder)
        {
            SolutionData solutionData = new SolutionData
            {
                Name = solutionInfo.Name,
                Caption = solutionInfo.Caption,
                IconFileName = solutionInfo.IconName,
                PostCreateUrl = solutionInfo.PostCreateUrl,
                Options = CopyOptions(solutionInfo.Options),
                Variants = solutionInfo.SolutionVariants
                    .Select(x => CreateSolutionVariant(x, destinationFolder))
                    .ToArray()
            };

            return solutionData;
        }

        private SolutionOptionsData CopyOptions(SolutionOptionsInfo info)
        {
            return new SolutionOptionsData
            {
                CanCreateTests = info.CanCreateTests,
                DefaultCreateTests = info.DefaultCreateTests,
                CanCreateFakes = info.CanCreateFakes,
                DefaultCreateFakes = info.DefaultCreateFakes,
                CanCreateSamples = info.CanCreateSamples,
                DefaultCreateSamples = info.DefaultCreateSamples,
                CanSupportNavigation = info.CanSupportNavigation,
                DefaultSupportNavigation = info.DefaultSupportNavigation,
                UseOnlyDefautValues = info.UseOnlyDefautValues,
            };
        }

        private SolutionVariantData CreateSolutionVariant(SolutionVariant solutionVariant, string destinationFolder)
        {
            SolutionVariantData solutionVariantData = new SolutionVariantData
            {
                ContainerName = solutionVariant.ContainerName,
                Items = solutionVariant.SolutionTemplateInfo.Items
                    .Select(info => CreateSolutionItemData(info, destinationFolder))
                    .ToArray()
            };

            return solutionVariantData;
        }

        private SolutionItemData CreateSolutionItemData(ISolutionItemTemplateInfo solutionItemInfo, string destinationFolder)
        {
            SolutionItemData solutionItemData;

            var solutionFolderInfo = solutionItemInfo as ISolutionFolderTemplateInfo;
            if (solutionFolderInfo != null)
            {
                solutionItemData = CreateSolutionFolderData(solutionFolderInfo, destinationFolder);
                solutionItemData.Name = solutionItemInfo.Name;
                return solutionItemData;
            }

            var projectInfo = solutionItemInfo as IProjectTemplateInfo;
            if (projectInfo != null)
            {
                solutionItemData = CreateProjectData(projectInfo, destinationFolder);
                return solutionItemData;
            }

            throw new ArgumentException("Unknown solution item type.", nameof(solutionItemInfo));
        }

        private SolutionFolderData CreateSolutionFolderData(ISolutionFolderTemplateInfo solutionFolderInfo, string destinationFolder)
        {
            SolutionFolderData solutionFolderData = new SolutionFolderData
            {
                Items = solutionFolderInfo.Items
                    .Select(info => CreateSolutionItemData(info, destinationFolder))
                    .ToArray()
            };
            return solutionFolderData;
        }

        private ProjectData CreateProjectData(IProjectTemplateInfo projectInfo, string destinationFolder)
        {
            ProjectData projectData = new ProjectData
            {
                Name = projectInfo.NameWithoutRoot,
                IsStartup = projectInfo.IsStartup,
                FileName = Utils.GetRelativePath(projectInfo.DestinationFileName, destinationFolder),
                ProjectConfigurations = CreateProjectConfigurations(projectInfo.ProjectConfigurations)
            };


            return projectData;
        }

        private ProjectConfigurationData[] CreateProjectConfigurations(IEnumerable<IProjectConfiguration> projectConfigurations)
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