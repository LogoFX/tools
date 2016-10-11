using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Threading;
using Caliburn.Micro;
using EnvDTE;
using EnvDTE100;
using EnvDTE80;
using LogoFX.Tools.Common.Model;
using LogoFX.Tools.Templates.Wizard.ViewModel;
using Microsoft.VisualStudio.TemplateWizard;

namespace LogoFX.Tools.Templates.Wizard
{
    public class ExperimentalSolutionWizard : SolutionWizard
    {
        private WizardData _wizardData;

        protected override string GetTitle()
        {
            return _wizardData.Title;
        }

        protected override WizardConfiguration GetWizardConfiguration()
        {
            WizardConfiguration wizardConfiguration = new WizardConfiguration
            {
                FakeOption = true,
                TestOption = true,
                Solutions = _wizardData.Solutions.Select(CreateSolutionInfo)
            };

            return wizardConfiguration;
        }

        private SolutionInfo CreateSolutionInfo(SolutionData solutionData)
        {
            SolutionInfo solutionInfo = new SolutionInfo
            {
                Name = solutionData.Name,
                Caption = solutionData.Caption,
                IconName = solutionData.IconFileName
            };
            return solutionInfo;
        }

        protected override void RunStartedOverride(Solution4 solution, Dictionary<string, string> replacementsDictionary, object[] customParams)
        {
            var vstemplateName = (string) customParams[0];
            var tmpFolder = Path.GetDirectoryName(vstemplateName);
            
            var wizardDataFileName = Path.Combine(tmpFolder, WizardDataLoader.WizardDataFielName);
            _wizardData = WizardDataLoader.LoadAsync(wizardDataFileName).Result;

            base.RunStartedOverride(solution, replacementsDictionary, customParams);
        }

        protected override void RunFinishedOverride()
        {
            var solution = GetSolution();
            var solutionInfo = GetSelectedSolutionInfo();

            var solutionData = _wizardData.Solutions.Single(x => x.Name == solutionInfo.Name);

            var waitViewModel = new WaitViewModel();
            var window = WpfServices.CreateWindow<Views.WaitView>(waitViewModel);
            WpfServices.SetWindowOwner(window, solution.DTE.MainWindow);
            var dispatcher = Dispatcher.CurrentDispatcher;
            waitViewModel.Completed += async (s, a) =>
            {
                await dispatcher.InvokeAsync(() =>
                {
                    window.DialogResult = !a.Cancelled && a.Error == null;
                });
            };
            waitViewModel.Caption = "Creating solution...";
            waitViewModel.Run((p, ct) =>
            {
                int count = solutionData.Items.Length;
                double k = 1.0/count;
                for (int i = 0; i < count; ++i)
                {
                    var pr = k*i;
                    p.Report(pr);
                    int j = i;
                    CreateSolutionItem(
                        null,
                        solutionData.Items[i], progress =>
                        {
                            pr = k*j + progress*k;
                            p.Report(pr);
                        },
                        ct);
                    ct.ThrowIfCancellationRequested();
                }
            });
            var retVal = window.ShowDialog() ?? false;
            if (!retVal)
            {
                throw new WizardCancelledException();
            }
        }

        private void CreateSolutionItem(
            SolutionFolder solutionFolder, 
            SolutionItemData solutionItemData, 
            Action<double> progressAction,
            CancellationToken ct)
        {
            var solutionFolderData = solutionItemData as SolutionFolderData;
            if (solutionFolderData != null)
            {
                CreateSolutionFolder(
                    solutionFolder, 
                    solutionFolderData,
                    progressAction,
                    ct);
                return;
            }

            var projectData = solutionItemData as ProjectData;
            if (projectData != null)
            {
                CreateProject(projectData, progressAction);
                return;
            }

            throw new ArgumentException("Unknown solution item type");
        }

        private void CreateSolutionFolder(
            SolutionFolder solutionFolder, 
            SolutionFolderData solutionFolderData,
            Action<double> progressAction,
            CancellationToken ct)
        {
            var addedProject = solutionFolder == null
                ? GetSolution().AddSolutionFolder(solutionFolderData.Name)
                : solutionFolder.AddSolutionFolder(solutionFolderData.Name);

            if (solutionFolderData.Items.Length == 0)
            {
                return;
            }

            SolutionFolder subFolder = addedProject.Object as SolutionFolder;
            var k = 1.0/solutionFolderData.Items.Length;
            for (int i = 0; i < solutionFolderData.Items.Length; ++i)
            {
                var pr = k * i;
                progressAction(pr);
                int j = i;
                CreateSolutionItem(
                    subFolder,
                    solutionFolderData.Items[i], progress =>
                    {
                        pr = k * j + progress * k;
                        progressAction(pr);
                    },
                    ct);
                ct.ThrowIfCancellationRequested();
            }
        }

        private void CreateProject(ProjectData projectData, Action<double> progressAction)
        {
            
        }
    }
}