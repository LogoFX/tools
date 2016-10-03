using System.Collections.Generic;
using System.Linq;
using EnvDTE;

namespace LogoFX.Tools.Templates.Wizard
{
    public sealed class PostSolutionWizard : SolutionWizardBase
    {

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
                GetSolution().Properties.Item("StartupProject").Value = startupProject.Name;
            }
        }

        protected override void RunFinishedOverride()
        {
            var projects = GetProjects();
            SetStartupProject(projects);
        }
    }
}