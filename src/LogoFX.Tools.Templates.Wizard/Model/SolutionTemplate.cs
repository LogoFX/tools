using System.Collections.Generic;
using System.IO;
using System.Linq;
using EnvDTE;

namespace LogoFX.Tools.Templates.Wizard.Model
{
    internal sealed class SolutionTemplate : SolutionItemCollectionTemplate<Solution>
    {
        public SolutionTemplate(Solution item) : base(item)
        {
        }

        protected override string GetNameOverride(Solution item)
        {
            return Path.GetFileNameWithoutExtension(item.FileName);
        }

        protected override IEnumerable<Project> GetProjects(Solution item)
        {
            return item.Projects.OfType<Project>().ToList();
        }
    }
}