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

namespace LogoFX.Tools.Templates.Wizard
{
    public abstract partial class SolutionWizard
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

        private IEnumerable<Project> ApplyWizardModifications(IEnumerable<Project> projects)
        {
            if (_wizardViewModel == null)
            {
                return projects;
            }

            projects = RemoveMultiSolution(_wizardViewModel.SelectedSolution.Model);

            //if (!_wizardViewModel.CreateTests)
            //{
            //    RemoveTestProjects();
            //}

            //if (!_wizardViewModel.CreateFakes)
            //{
            //    RemoveFakesProjects(projects);
            //}

            if (!_wizardViewModel.MustRemoveCondition)
            {
                RemoveConditions(projects);
            }

            return projects;
        }

        private void RemoveConditions(IEnumerable<Project> projects)
        {
            foreach (var project in projects)
            {
                RemoveConditions(project);
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
            if (parent == null && solutionFolder.Name == "Tests")
            {
                return;
            }

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
    }
}