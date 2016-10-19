using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using EnvDTE;
using EnvDTE100;
using EnvDTE80;
using LogoFX.Tools.Common.Model;
using LogoFX.Tools.Templates.Wizard.Model;
using LogoFX.Tools.Templates.Wizard.ViewModel;
using LogoFX.Tools.Templates.Wizard.Views;
using Microsoft.Build.Construction;
using Microsoft.VisualStudio.TemplateWizard;

namespace LogoFX.Tools.Templates.Wizard
{
    public sealed class SolutionWizard : SolutionWizardBase
    {
        #region Fields

        private Dictionary<string, string> _replacementsDictionary;
        private WizardDataViewModel _wizardDataViewModel;
        private string _tmpFolder;

        #endregion

        #region Private Members

        private Project[] RemoveConditions(Project[] projects)
        {
            foreach (var project in projects)
            {
                RemoveConditions(project);
            }

            return projects;
        }

        private void RemoveConditions(Project project)
        {
            var projectFilePath = project.FileName;

            if (!string.Equals(Path.GetExtension(projectFilePath), ".csproj"))
            {
                return;
            }

            var buildProject = new Microsoft.Build.Evaluation.Project(projectFilePath);
            var allGroups = buildProject.Xml.PropertyGroups;

            var toRemove = new List<ProjectPropertyGroupElement>();

            if (!_wizardDataViewModel.CreateFakes)
            {
                toRemove.AddRange(allGroups.Where(x => x.Condition.Contains("Fake")));
            }

            if (!_wizardDataViewModel.CreateFakes)
            {
                toRemove.AddRange(allGroups.Where(x => x.Condition.Contains("Tests")));
            }

            if (toRemove.Count > 0)
            {
                foreach (var pg in toRemove.Distinct())
                {
                    buildProject.Xml.RemoveChild(pg);
                }
                buildProject.Save();
            }
        }

        private void AddProjectToSolution(SolutionFolder parent, SolutionItemTemplate project, IList<Project> projects)
        {
            if (project is SolutionFolderTemplate)
            {
                AddSolutionFolder(parent, (SolutionFolderTemplate)project, projects);
            }
            else
            {
                AddProject(parent, (ProjectTemplate)project, projects);
            }
        }

        private void AddSolutionFolder(SolutionFolder parent, SolutionFolderTemplate solutionFolder, IList<Project> projects)
        {
            if (!_wizardDataViewModel.CreateTests && 
                parent == null && 
                solutionFolder.Name == "Tests")
            {
                return;
            }

            if (!_wizardDataViewModel.CreateFakes &&
                solutionFolder.Name == "Fake")
            {
                return;
            }

            var addedProject = parent == null
                ? GetSolution().AddSolutionFolder(solutionFolder.Name)
                : parent.AddSolutionFolder(solutionFolder.Name);

            foreach (var item in solutionFolder.Items)
            {
                AddProjectToSolution(addedProject.Object as SolutionFolder, item, projects);
            }
        }

        private void AddProject(SolutionFolder parent, ProjectTemplate project, IList<Project> projects)
        {
            var projectFullName = project.FileName;
            var projectFileName = Path.GetFileName(projectFullName);
            var projectDirectory = Path.GetDirectoryName(projectFullName);

            Debug.Assert(projectDirectory != null, "projectDirectory != null");

            var directoryName = Path.GetDirectoryName(projectDirectory);
            var path1 = Path.GetDirectoryName(directoryName);

            Debug.Assert(path1 != null, "path1 != null");

            var newProjectDirectory = Path.Combine(path1, Path.GetFileName(projectDirectory));

            Debug.Assert(projectFileName != null, "projectFileName != null");

            var newProjectFullName = Path.Combine(newProjectDirectory, projectFileName);

            CopyDirectory(projectDirectory, newProjectDirectory);

            var addedProject = parent == null
                ? GetSolution().AddFromFile(newProjectFullName)
                : parent.AddFromFile(newProjectFullName);

            projects.Add(addedProject);
        }

        public bool CreateTests
        {
            get { return _wizardDataViewModel.CreateTests; }
        }

        public bool CreateFakes
        {
            get { return _wizardDataViewModel.CreateFakes; }
        }

        /// <summary>
        /// Copies the directory.
        /// </summary>
        /// <param name="sourceDirectory">The source directory.</param>
        /// <param name="destDirectory">The destination directory.</param>
        private static void CopyDirectory(string sourceDirectory, string destDirectory)
        {
            if (!Directory.Exists(destDirectory))
            {
                Directory.CreateDirectory(destDirectory);
            }

            var files = Directory.GetFiles(sourceDirectory);
            foreach (var file in files)
            {
                var name = Path.GetFileName(file);
                if (name == null) continue;

                var dest = Path.Combine(destDirectory, name);
                File.Copy(file, dest);
            }

            var folders = Directory.GetDirectories(sourceDirectory);
            foreach (var folder in folders)
            {
                var name = Path.GetFileName(folder);
                if (name == null) continue;

                var dest = Path.Combine(destDirectory, name);
                CopyDirectory(folder, dest);
            }
        }

        private void CreateSolution(SolutionVariantData solutionVariantData)
        {
            var solution = GetSolution();

            var waitViewModel = new WaitViewModel();
            var window = WpfServices.CreateWindow<WaitView>(waitViewModel);
            WpfServices.SetWindowOwner(window, solution.DTE.MainWindow);
            waitViewModel.Completed += async (s, a) =>
            {
                await window.Dispatcher.InvokeAsync(() =>
                {
                    window.DialogResult = !a.Cancelled && a.Error == null;
                });
            };
            waitViewModel.Caption = "Creating solution...";
            waitViewModel.Run((p, ct) =>
            {
                int count = solutionVariantData.Items.Length;
                double k = 1.0 / count;
                for (int i = 0; i < count; ++i)
                {
                    var pr = k * i;
                    p.Report(pr);
                    int j = i;
                    CreateSolutionItem(
                        null,
                        solutionVariantData.Items[i], progress =>
                        {
                            pr = k * j + progress * k;
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
                CreateProject(
                    solutionFolder,
                    projectData,
                    progressAction,
                    ct);
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
            if (!CreateTests &&
                solutionFolder == null &&
                solutionFolderData.Name == "Tests")
            {
                return;
            }

            if (!CreateFakes &&
                solutionFolderData.Name == "Fake")
            {
                return;
            }

            var addedProject = solutionFolder == null
                ? GetSolution().AddSolutionFolder(solutionFolderData.Name)
                : solutionFolder.AddSolutionFolder(solutionFolderData.Name);

            if (solutionFolderData.Items.Length == 0)
            {
                return;
            }

            SolutionFolder subFolder = addedProject.Object as SolutionFolder;
            var k = 1.0 / solutionFolderData.Items.Length;
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

        private void CreateProject(
            SolutionFolder solutionFolder,
            ProjectData projectData,
            Action<double> progressAction,
            CancellationToken ct)
        {
            var projectName = $"{_replacementsDictionary["$safeprojectname$"]}.{projectData.Name}";

            var sourceFileName = Path.Combine(_tmpFolder, projectData.FileName);
            var sourceDir = Path.GetDirectoryName(sourceFileName);
            var solutionDir = _replacementsDictionary["$solutiondirectory$"];
            var destDir = Path.Combine(solutionDir, projectName);

            var replacementDictionary = CreateProjectReplacementDictionary(projectName);
            CopyDirectory(replacementDictionary, sourceDir, destDir, progressAction, ct);

            var oldFileName = Path.GetFileName(sourceFileName);
            var ext = Path.GetExtension(oldFileName);
            var newFileName = projectName + ext;
            var newFullFileName = Path.Combine(destDir, newFileName);

            File.Move(Path.Combine(destDir, oldFileName), newFullFileName);

            var addedProject = solutionFolder == null
                ? GetSolution().AddFromFile(newFullFileName)
                : solutionFolder.AddFromFile(newFullFileName);

            foreach (SolutionConfiguration solutionConfiguration in GetSolution().SolutionBuild.SolutionConfigurations)
            {
                var solutionContext = solutionConfiguration.SolutionContexts
                    .OfType<SolutionContext>()
                    .Single(x => x.ProjectName.EndsWith(newFileName));

                Debug.WriteLine("Project Name: " + projectName);

                var name = solutionConfiguration.Name;
                name = $"{name}|{solutionContext.PlatformName}";

                var projectConfiguration = projectData.ProjectConfigurations.SingleOrDefault(x => x.Name == name);

                Debug.Assert(projectConfiguration != null, "ProjectConfiguration not found for " + name);

                solutionContext.ConfigurationName = projectConfiguration.ConfigurationName;
                solutionContext.ShouldBuild = projectConfiguration.IncludeInBuild;
            }

            if (projectData.IsStartup)
            {
                GetSolution().Properties.Item("StartupProject").Value = addedProject.Name;
            }

            progressAction(1.0);
        }

        private Dictionary<string, string> CreateProjectReplacementDictionary(string projectName)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (var key in _replacementsDictionary.Keys)
            {
                result[key] = _replacementsDictionary[key];
            }

            result["$safeprojectname$"] = projectName;
            result["$projectname$"] = projectName;

            return result;
        }

        private void CopyDirectory(
            Dictionary<string, string> replacementsDictionary,
            string sourceDir,
            string destDir,
            Action<double> progressAction,
            CancellationToken ct)
        {
            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }

            var infos = new DirectoryInfo(sourceDir).GetFileSystemInfos();
            if (infos.Length == 0)
            {
                return;
            }

            var k = 0.8 / infos.Length;

            for (int i = 0; i < infos.Length; ++i)
            {
                ct.ThrowIfCancellationRequested();

                var pr = k * i;
                progressAction(pr);
                int j = i;

                var info = infos[i];

                var destFileName = Path.Combine(destDir, info.Name);
                if (info is DirectoryInfo)
                {
                    CopyDirectory(
                        replacementsDictionary,
                        info.FullName,
                        destFileName,
                        progress =>
                        {
                            pr = k * j + progress * k;
                            progressAction(pr);
                        },
                        ct);
                    continue;
                }

                CopyProject(replacementsDictionary, info.FullName, destFileName);
            }
        }

        private void CopyProject(
            Dictionary<string, string> replacementsDictionary,
            string oldFileName,
            string newFileName)
        {
            ReplaceText(replacementsDictionary, oldFileName, newFileName);
        }

        private void ReplaceText(
            Dictionary<string, string> replacementsDictionary,
            string oldFileName,
            string newFileName)
        {
            var str = File.ReadAllText(oldFileName);
            foreach (var sp in replacementsDictionary)
            {
                str = str.Replace(sp.Key, sp.Value);
            }
            File.WriteAllText(newFileName, str);
        }

        private Project[] ApplyWizardModifications(Project[] projects)
        {
            if (_wizardDataViewModel == null)
            {
                return projects;
            }

            if (_wizardDataViewModel.MustRemoveConditions)
            {
                projects = RemoveConditions(projects);
            }

            return projects;
        }

        #endregion

        #region Overrides

        protected override void RunStartedOverride(Solution4 solution, Dictionary<string, string> replacementsDictionary, object[] customParams)
        {
            _replacementsDictionary = replacementsDictionary;

            var vstemplateName = (string)customParams[0];
            _tmpFolder = Path.GetDirectoryName(vstemplateName);
            replacementsDictionary["$saferootprojectname$"] = replacementsDictionary["$safeprojectname$"];

            var wizardDataFileName = Path.Combine(_tmpFolder, WizardDataLoader.WizardDataFielName);
            var wizardData = WizardDataLoader.LoadAsync(wizardDataFileName).Result;

            _wizardDataViewModel = null;

            if (wizardData == null ||
                !wizardData.ShowWizardWindow())
            {
                return;
            }

            var projectName = replacementsDictionary["$projectname$"];

            _wizardDataViewModel = new WizardDataViewModel(wizardData)
            {
                Title = $"{wizardData.Title} - {projectName}"
            };

            var window = WpfServices.CreateWindow<WizardWindow>(_wizardDataViewModel);
            WpfServices.SetWindowOwner(window, solution.DTE.MainWindow);
            var retVal = window.ShowDialog() ?? false;
            if (!retVal)
            {
                throw new WizardCancelledException();
            }
        }

        protected override void RunFinishedOverride()
        {
            var solutionData = _wizardDataViewModel.SelectedSolution.Model;
            var solutionVariant = _wizardDataViewModel.SelectedSolution.SelectedVariant.Model;

            CreateSolution(solutionVariant);

            if (!string.IsNullOrWhiteSpace(solutionData.PostCreateUrl))
            {
                GetSolution().DTE.ItemOperations.Navigate(solutionData.PostCreateUrl);
            }

            //Get all projects in solution
            var projects = GetProjects().ToArray();
            if (projects == null || !projects.Any())
            {
                throw new Exception("No projects found.");
            }

            ApplyWizardModifications(projects);
        }

        #endregion
    }
}