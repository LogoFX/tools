using System.Collections.Generic;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using TemplateBuilder;

namespace LogoFX.Tools.Templates.Wizard
{
    public sealed class ProjectWizard : IWizard
    {
        private readonly IWizard _childWizard = new ChildWizard();

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            _childWizard.RunStarted(automationObject, replacementsDictionary, runKind, customParams);
        }

        public void ProjectFinishedGenerating(Project project)
        {
            _childWizard.ProjectFinishedGenerating(project);
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
            _childWizard.ProjectItemFinishedGenerating(projectItem);
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return _childWizard.ShouldAddProjectItem(filePath);
        }

        public void BeforeOpeningFile(ProjectItem projectItem)
        {
            _childWizard.BeforeOpeningFile(projectItem);
        }

        public void RunFinished()
        {
            _childWizard.RunFinished();
        }
    }
}