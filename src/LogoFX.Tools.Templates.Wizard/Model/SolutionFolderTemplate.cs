using System.Collections.Generic;
using EnvDTE;

namespace LogoFX.Tools.Templates.Wizard.Model
{
    internal class SolutionFolderTemplate : SolutionItemCollectionTemplate<Project>
    {
        public SolutionFolderTemplate(Project item) 
            : base(item)
        {
        }

        protected override string GetNameOverride(Project item)
        {
            return item.Name;
        }

        protected override IEnumerable<Project> GetProjects(Project item)
        {
            List<Project> projects = new List<Project>();
            var projectItems = item.ProjectItems;
            for (int i = 1; i <= projectItems.Count; ++i)
            {
                var proj = projectItems.Item(i).SubProject;
                projects.Add(proj);
            }

            return projects;
        }
    }
}