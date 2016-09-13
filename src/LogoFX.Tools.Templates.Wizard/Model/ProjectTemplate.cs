using EnvDTE;

namespace LogoFX.Tools.Templates.Wizard.Model
{
    internal sealed class ProjectTemplate : SolutionItemTemplate<Project>
    {
        public ProjectTemplate(Project item) 
            : base(item)
        {
            FileName = item.FileName;
        }

        public string FileName { get; }

        protected override string GetNameOverride(Project item)
        {
            return item.Name;
        }
    }
}