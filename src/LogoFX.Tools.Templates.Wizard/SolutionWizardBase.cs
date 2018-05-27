using System.Collections.Generic;
using System.Diagnostics;
using EnvDTE;
using EnvDTE100;
using EnvDTE80;
using Microsoft.VisualStudio.TemplateWizard;

namespace LogoFX.Tools.Templates.Wizard
{
    public abstract class SolutionWizardBase : IWizard
    {
        #region Fields

        private const string SolutionFolderKind = "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}";

        private Solution4 _solution;

        #endregion

        #region Protected Methods

        protected IList<Project> GetProjects()
        {
            Projects projects = GetSolution().Projects;
            List<Project> projectList = new List<Project>();
            foreach (object obj in projects)
            {
                Project solutionFolder = obj as Project;
                AddProjectsToList(solutionFolder, projectList);
            }
            return projectList;
        }

        protected virtual void RunStartedOverride(Solution4 solution, Dictionary<string, string> replacementsDictionary, object[] customParams)
        {
            
        }

        protected virtual void RunFinishedOverride()
        {
            
        }

        protected Solution4 GetSolution()
        {
            return _solution;
        }

        #endregion

        #region Private Members

        private IEnumerable<Project> GetSolutionFolderProjects(Project solutionFolder)
        {
            List<Project> projectList = new List<Project>();
            for (int index = 1; index <= solutionFolder.ProjectItems.Count; ++index)
            {
                Project subProject = solutionFolder.ProjectItems.Item(index).SubProject;
                AddProjectsToList(subProject, projectList);
            }
            return projectList;
        }

        private void AddProjectsToList(Project rootProject, List<Project> projectList)
        {
            if (rootProject != null)
            {
                if (rootProject.Kind == SolutionFolderKind)
                {
                    projectList.AddRange(GetSolutionFolderProjects(rootProject));
                }
                else
                {
                    projectList.Add(rootProject);
                }
            }
        }

        #endregion

        #region IWizard

        void IWizard.RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            if (runKind != WizardRunKind.AsMultiProject)
            {
                return;
            }

            DTE2 dtE2 = automationObject as DTE2;
            Debug.Assert(dtE2 != null, nameof(dtE2) + " != null");
            // ReSharper disable SuspiciousTypeConversion.Global
            var solution4 = dtE2.Solution as Solution4;
            if (solution4 != null)
            {
                _solution = (Solution4)dtE2.Solution;
            }
            // ReSharper restore SuspiciousTypeConversion.Global

            RunStartedOverride(solution4, replacementsDictionary, customParams);
        }

        void IWizard.ProjectFinishedGenerating(Project project)
        {

        }

        void IWizard.ProjectItemFinishedGenerating(ProjectItem projectItem)
        {

        }

        bool IWizard.ShouldAddProjectItem(string filePath)
        {
            return true;
        }

        void IWizard.BeforeOpeningFile(ProjectItem projectItem)
        {

        }

        void IWizard.RunFinished()
        {
            RunFinishedOverride();
        }

        #endregion
    }
}