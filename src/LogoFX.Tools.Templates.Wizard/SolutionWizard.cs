using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using EnvDTE;
using EnvDTE100;
using EnvDTE80;
using LogoFX.Tools.Templates.Wizard.Model;
using LogoFX.Tools.Templates.Wizard.ViewModel;
using Microsoft.Build.Construction;
using Microsoft.VisualStudio.TemplateWizard;

namespace LogoFX.Tools.Templates.Wizard
{
    public abstract partial class SolutionWizard : SolutionWizardBase
    {
        #region Fields

        private WizardViewModel _wizardViewModel;

        #endregion

        #region Public Propeties

        public string Title => GetTitle();

        #endregion

        #region Protected

        protected abstract string GetTitle();

        protected abstract WizardConfiguration GetWizardConfiguration();

        #endregion

        #region Private Members

        private IEnumerable<Project> ApplyWizardModifications(IEnumerable<Project> projects)
        {
            if (_wizardViewModel == null)
            {
                return projects;
            }

            projects = RemoveMultiSolution(_wizardViewModel.SelectedSolution.Model);

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

        private IEnumerable<Project> RemoveMultiSolution(SolutionInfo solutionInfo)
        {
            SolutionFolderTemplate selected = null;
            foreach (var p in GetSolution().Projects.OfType<Project>().ToList())
            {
                if (p.Name == solutionInfo.Name)
                {
                    selected = new SolutionFolderTemplate(p);
                }

                GetSolution().Remove(p);
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
            if (!_wizardViewModel.CreateTests && 
                parent == null && 
                solutionFolder.Name == "Tests")
            {
                return;
            }

            if (!_wizardViewModel.CreateFakes &&
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
            var newProjectDirectory = Path.Combine(
                Path.GetDirectoryName(
                    Path.GetDirectoryName(projectDirectory)),
                Path.GetFileName(projectDirectory));
            var newProjectFullName = Path.Combine(newProjectDirectory, projectFileName);

            CopyDirectory(projectDirectory, newProjectDirectory);
            //DeleteDirectory(projectDirectory);

            var addedProject = parent == null
                ? GetSolution().AddFromFile(newProjectFullName)
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

        #endregion

        #region Overrides

        protected override void RunStartedOverride(Solution4 solution, Dictionary<string, string> replacementsDictionary, object[] customParams)
        {
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
            WpfServices.SetWindowOwner(window, solution.DTE.MainWindow);
            var retVal = window.ShowDialog() ?? false;
            if (!retVal)
            {
                throw new WizardCancelledException();
            }
        }

        protected override void RunFinishedOverride()
        {
            //Get all projects in solution
            IEnumerable<Project> projects = GetProjects().ToList();
            if (projects == null || !projects.Any())
            {
                throw new Exception("No projects found.");
            }

            ApplyWizardModifications(projects);
        }

        #endregion
    }
}