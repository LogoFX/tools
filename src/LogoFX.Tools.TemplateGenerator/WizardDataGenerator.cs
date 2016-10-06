using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LogoFX.Tools.Common.Model;
using LogoFX.Tools.TemplateGenerator.Contracts;

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
                Caption = solutionInfo.Caption,
                IconFileName = solutionInfo.IconName,
                Name = solutionInfo.Name,
                Items = solutionInfo.SolutionTemplateInfo.Items
                    .Select(info => CreateSolutionItemData(info, destinationFolder))
                    .ToArray()
            };

            return solutionData;
        }

        private SolutionItemData CreateSolutionItemData(ISolutionItemTemplateInfo solutionItemInfo, string destinationFolder)
        {
            SolutionItemData solutionItemData = null;

            var solutionFolderInfo = solutionItemInfo as ISolutionFolderTemplateInfo;
            if (solutionFolderInfo != null)
            {
                solutionItemData = CreateSolutionFolderData(solutionFolderInfo, destinationFolder);
            }

            var projectInfo = solutionItemInfo as IProjectTemplateInfo;
            if (projectInfo != null)
            {
                solutionItemData = CreateProjectData(projectInfo, destinationFolder);
            }

            if (solutionItemData == null)
            {
                throw new ArgumentException("Unknown solution item type.", nameof(solutionItemInfo));
            }

            solutionItemData.Name = solutionItemInfo.Name;

            return solutionItemData;
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
            ProjectData projectData = new ProjectData();
            projectData.IsStartupProject = projectInfo.Name.EndsWith(".Launcher");
            projectData.FileName = Utils.GetRelativePath(projectInfo.DestinationFileName, destinationFolder);

            return projectData;
        }
    }
}