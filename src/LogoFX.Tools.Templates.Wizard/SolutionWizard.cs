using System.Collections.Generic;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;

namespace LogoFX.Tools.Templates.Wizard
{
    public class SolutionWizard : IWizard
    {
        private readonly TemplateBuilder.SolutionWizard _solutionWizard = 
            new TemplateBuilder.SolutionWizard();

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            var form = new UserInputForm();
            form.ShowDialog();

            _solutionWizard.RunStarted(automationObject, replacementsDictionary, runKind, customParams);
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
            _solutionWizard.ProjectFinishedGenerating(project);
        }
    }
}