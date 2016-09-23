using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using EnvDTE;
using EnvDTE100;
using EnvDTE80;
using LogoFX.Tools.TemplateGenerator;
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

        protected abstract WizardConfiguration GetWizardConfiguration();

        #endregion

        #region Private Members

        private void RemoveConditions()
        {
            var projects = GetProjects();
            foreach (var project in projects)
            {
                RemoveConditions(project);
            }
        }

        private void ApplyWizardModifications()
        {
            if (_wizardViewModel == null)
            {
                return;
            }

            if (_wizardViewModel.SelectedSolution != null)
            {
                RemoveMultiSolution(_wizardViewModel.SelectedSolution.Model);
            }

            if (!_wizardViewModel.CreateTests)
            {
                RemoveTestProjects();
            }

            if (!_wizardViewModel.CreateFakes)
            {
                RemoveFakesProjects();
            }

            if (!_wizardViewModel.CreateTests || !_wizardViewModel.CreateFakes)
            {
                RemoveConditions();
            }
        }

        private void RemoveMultiSolution(SolutionInfo solutionInfo)
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

            Debug.Assert(selected != null, "selected != null");
            foreach (var p in selected.Items)
            {
                AddProjectToSolution(null, p);
            }
        }

        private void AddProjectToSolution(SolutionFolder parent, SolutionItemTemplate project)
        {
            if (project is SolutionFolderTemplate)
            {
                AddSolutionFolder(parent, (SolutionFolderTemplate)project);
            }
            else
            {
                AddProject(parent, (ProjectTemplate)project);
            }
        }

        private void AddSolutionFolder(SolutionFolder parent, SolutionFolderTemplate solutionFolder)
        {
            var addedProject = parent == null
                ? _solution.AddSolutionFolder(solutionFolder.Name)
                : parent.AddSolutionFolder(solutionFolder.Name);

            foreach (var item in solutionFolder.Items)
            {
                AddProjectToSolution(addedProject.Object as SolutionFolder, item);
            }
        }

        private void AddProject(SolutionFolder parent, ProjectTemplate project)
        {
            var addedProject = parent == null
                ? _solution.AddFromFile(project.FileName)
                : parent.AddFromFile(project.FileName);
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

        private void RemoveFakesProjects()
        {
            var projects = GetProjects(true);
            foreach (var p in projects.Where(p => p.Name.Contains(".Fake.")))
            {
                _solution.Remove(p);
            }
        }

        private void SetStartupProject()
        {
            var projects = GetProjects(true);
            var startupProjectName = projects.First(x => x.Name.EndsWith("Launcher")).Name;

            if (!string.IsNullOrEmpty(startupProjectName))
            {
                _solution.Properties.Item("StartupProject").Value = startupProjectName;
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
            ApplyWizardModifications();
            SetStartupProject();
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