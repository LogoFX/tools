using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using EnvDTE;
using EnvDTE100;
using EnvDTE80;
using LogoFX.Tools.Common;
using LogoFX.Tools.Templates.Wizard.Model;
using LogoFX.Tools.Templates.Wizard.ViewModel;
using Microsoft.Build.Construction;
using Microsoft.VisualStudio.TemplateWizard;

namespace LogoFX.Tools.Templates.Wizard
{
    public abstract class SolutionWizard : IWizard
    {
        #region Fields

        private const string SolutionFolderKind = "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}";

        private WizardViewModel _wizardViewModel;

        private Solution4 _solution;

        #endregion

        #region Public Propeties

        public string Title => GetTitle();

        #endregion

        #region Protected

        protected abstract string GetTitle();

        protected abstract WizardConfigurationDto GetWizardConfiguration();

        #endregion

        #region Private Members

        private void RemoveConditions(IEnumerable<Project> projects)
        {
            foreach (var project in projects)
            {
                RemoveConditions(project);
            }
        }

        private IEnumerable<Project> ApplyWizardModifications(IEnumerable<Project> projects)
        {
            if (_wizardViewModel == null)
            {
                return projects;
            }

            return RemoveMultiSolution(_wizardViewModel.SelectedSolution.Model);

            //if (!_wizardViewModel.CreateTests)
            //{
            //    RemoveTestProjects();
            //}

            //if (!_wizardViewModel.CreateFakes)
            //{
            //    RemoveFakesProjects(projects);
            //}

            //if (!_wizardViewModel.CreateTests || !_wizardViewModel.CreateFakes)
            //{
            //    RemoveConditions(projects);
            //}
        }

        private IEnumerable<Project> RemoveMultiSolution(SolutionInfoDto solutionInfo)
        {
            SolutionFolderTemplate selected = null;
            foreach (var p in _solution.Projects.OfType<Project>().ToList())
            {
                if (p.Name == solutionInfo.Name)
                {
                    selected = new SolutionFolderTemplate(p);
                }

                _solution.Remove(p);
            }

            List<Project> projects = new List<Project>();

            Debug.Assert(selected != null, "selected != null");
            foreach (var p in selected.Items)
            {
                AddProjectToSolution(null, p, projects);
            }

            return projects;
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
            var addedProject = parent == null
                ? _solution.AddSolutionFolder(solutionFolder.Name)
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
            var newProjectDirectory = Path.Combine(
                Path.GetDirectoryName(
                    Path.GetDirectoryName(projectDirectory)),
                Path.GetFileName(projectDirectory));
            var newProjectFullName = Path.Combine(newProjectDirectory, projectFileName);

            CopyDirectory(projectDirectory, newProjectDirectory);
            //DeleteDirectory(projectDirectory);

            var addedProject = parent == null
                ? _solution.AddFromFile(newProjectFullName)
                : parent.AddFromFile(newProjectFullName);

            projects.Add(addedProject);
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

        /// <summary>
        /// Deletes the directory.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="inUseRetryCount">The in use retry count.</param>
        private static void DeleteDirectory(string directory, int inUseRetryCount = 3)
        {
            var counter = 0;
            while (counter < inUseRetryCount)
            {
                try
                {
                    if (!Directory.Exists(directory))
                    {
                        return;
                    }

                    var files = Directory.GetFiles(directory);
                    foreach (var file in files)
                    {
                        File.Delete(file);
                    }

                    var folders = Directory.GetDirectories(directory);
                    foreach (var folder in folders)
                    {
                        var name = Path.GetFileName(folder);
                        if (name == null) continue;

                        DeleteDirectory(folder);
                    }

                    Directory.Delete(directory);
                    break;
                }
                catch (Exception)
                {
                    System.Threading.Thread.Sleep(2000);
                    counter++;
                }
            }
        }

        private void RemoveTestProjects()
        {
            var testSolutionFolder = _solution.Projects
                .OfType<Project>()
                .FirstOrDefault(p => p.Name == "Tests");
            if (testSolutionFolder != null)
            {
                _solution.Remove(testSolutionFolder);
            }
        }

        private void RemoveFakesProjects(IEnumerable<Project> projects)
        {
            foreach (var p in projects.Where(p => p.Name.Contains(".Fake.")))
            {
                _solution.Remove(p);
            }
        }

        private void SetStartupProject(IEnumerable<Project> projects)
        {
            var startupProject = projects.FirstOrDefault(x => x.Name.EndsWith("Launcher"));
            if (startupProject == null)
            {
                startupProject = projects.FirstOrDefault(x => x.Name.EndsWith("Shell"));
            }
            if (startupProject == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(startupProject.Name))
            {
                _solution.Properties.Item("StartupProject").Value = startupProject.Name;
            }
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

            if (!_wizardViewModel.FakeOption)
            {
                toRemove.AddRange(allGroups.Where(x => x.Condition.Contains("Fake")));
            }

            if (!_wizardViewModel.TestOption)
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

        private IList<Project> GetProjects(bool allowSolutionFolders = false)
        {
            Projects projects = _solution.Projects;
            List<Project> projectList = new List<Project>();
            foreach (object obj in projects)
            {
                Project solutionFolder = obj as Project;
                AddProjectsToList(solutionFolder, projectList, allowSolutionFolders);
            }
            return projectList;
        }

        private IEnumerable<Project> GetSolutionFolderProjects(Project solutionFolder, bool allowSolutionFolders)
        {
            List<Project> projectList = new List<Project>();
            for (int index = 1; index <= solutionFolder.ProjectItems.Count; ++index)
            {
                Project subProject = solutionFolder.ProjectItems.Item(index).SubProject;
                AddProjectsToList(subProject, projectList, allowSolutionFolders);
            }
            return projectList;
        }

        private void AddProjectsToList(Project rootProject, List<Project> projectList, bool allowSolutionFolders)
        {
            if (rootProject != null)
            {
                if (rootProject.Kind == SolutionFolderKind)
                {
                    projectList.AddRange(GetSolutionFolderProjects(rootProject, allowSolutionFolders));
                    if (allowSolutionFolders)
                    {
                        projectList.Add(rootProject);
                    }
                }
                else
                {
                    projectList.Add(rootProject);
                }
            }
        }

        #endregion

        #region IWizard

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            if (runKind != WizardRunKind.AsMultiProject)
            {
                return;
            }

            DTE2 dtE2 = automationObject as DTE2;
            // ReSharper disable SuspiciousTypeConversion.Global
            var solution4 = dtE2.Solution as Solution4;
            if (solution4 != null)
            {
                _solution = (Solution4)dtE2.Solution;
            }
            // ReSharper restore SuspiciousTypeConversion.Global

            _wizardViewModel = null;

            var wizardConfiguration = GetWizardConfiguration();
            if (wizardConfiguration == null ||
                !wizardConfiguration.ShowWizardWindow())
            {
                return;
            }

            var projectName = replacementsDictionary["$projectname$"];

            _wizardViewModel = new WizardViewModel(wizardConfiguration)
            {
                Title = $"{Title} - {projectName}"
            };

            var window = WpfServices.CreateWindow<Views.WizardWindow>(_wizardViewModel);
            WpfServices.SetWindowOwner(window, dtE2.MainWindow);
            var retVal = window.ShowDialog() ?? false;
            if (!retVal)
            {
                throw new WizardCancelledException();
            }
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }

        public void RunFinished()
        {
            //Get all projects in solution
            IEnumerable<Project> projects = GetProjects().ToList();
            if (projects == null || !projects.Any())
            {
                throw new Exception("No projects found.");
            }

            projects = ApplyWizardModifications(projects);
            SetStartupProject(projects);
        }

        public void BeforeOpeningFile(ProjectItem projectItem)
        {

        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {

        }

        public void ProjectFinishedGenerating(Project project)
        {

        }

        #endregion
    }
}