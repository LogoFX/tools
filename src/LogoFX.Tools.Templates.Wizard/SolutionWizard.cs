using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE100;
using EnvDTE80;
using Microsoft.VisualStudio.TemplateWizard;

namespace LogoFX.Tools.Templates.Wizard
{
    public sealed class SolutionWizard : IWizard
    {
        private const string Title = "New LogoFX WPF Samples Application";

        private readonly TemplateBuilder.SolutionWizard _solutionWizard = 
            new TemplateBuilder.SolutionWizard();

        private readonly WizardData _wizardData = new WizardData();

        private Solution4 _solution;

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            _solutionWizard.RunStarted(automationObject, replacementsDictionary, runKind, customParams);

            if (runKind != WizardRunKind.AsMultiProject)
            {
                return;
            }

            DTE2 dtE2 = automationObject as DTE2;
            if (dtE2 != null)
            {
                _solution = (Solution4) dtE2.Solution;
            }

            var projectName = replacementsDictionary["$projectname$"];
            var form = new UserInputForm
            {
                Text = $"{Title} - {projectName}"
            };

            form.DataToScreen(_wizardData);
            var res = form.ShowDialog() == DialogResult.OK;
            form.ScreenToData(_wizardData);
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return _solutionWizard.ShouldAddProjectItem(filePath);
        }

        public void RunFinished()
        {
            _solutionWizard.RunFinished();
        }

        public void BeforeOpeningFile(ProjectItem projectItem)
        {
            _solutionWizard.BeforeOpeningFile(projectItem);
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
            _solutionWizard.ProjectItemFinishedGenerating(projectItem);
        }

        public void ProjectFinishedGenerating(Project project)
        {
            if (!_wizardData.Tests)
            {
                var testSolutionFolder = _solution.Projects
                    .OfType<Project>()
                    .FirstOrDefault(p => p.Name == "Tests");
                if (testSolutionFolder != null)
                {
                    _solution.Remove(testSolutionFolder);
                }
            }

            if (!_wizardData.FakeData)
            {
                var projects = GetProjects();
                foreach (var p in projects.Where(p => p.Name.Contains(".Fake.")))
                {
                    _solution.Remove(p);
                }
            }

            _solutionWizard.ProjectFinishedGenerating(project);
        }

        private IList<Project> GetProjects()
        {
            Projects projects = this._solution.Projects;
            List<Project> projectList = new List<Project>();
            foreach (object obj in projects)
            {
                Project solutionFolder = obj as Project;
                if (solutionFolder != null)
                {
                    if (solutionFolder.Kind == "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}")
                        projectList.AddRange(GetSolutionFolderProjects(solutionFolder));
                    else
                        projectList.Add(solutionFolder);
                }
            }
            return projectList;
        }

        private static IEnumerable<Project> GetSolutionFolderProjects(Project solutionFolder)
        {
            List<Project> projectList = new List<Project>();
            for (int index = 1; index <= solutionFolder.ProjectItems.Count; ++index)
            {
                Project subProject = solutionFolder.ProjectItems.Item(index).SubProject;
                if (subProject != null)
                {
                    if (subProject.Kind == "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}")
                        projectList.AddRange(GetSolutionFolderProjects(subProject));
                    else
                        projectList.Add(subProject);
                }
            }
            return projectList;
        }
    }
}