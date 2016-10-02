using System;
using System.Collections.Generic;
using System.Linq;
using EnvDTE;
using EnvDTE100;
using EnvDTE80;
using LogoFX.Tools.Templates.Wizard.ViewModel;
using Microsoft.VisualStudio.TemplateWizard;

namespace LogoFX.Tools.Templates.Wizard
{
    public abstract partial class SolutionWizard : IWizard
    {
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
    }
}