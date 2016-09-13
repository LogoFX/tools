using System;
using EnvDTE;

namespace LogoFX.Tools.Templates.Wizard.Model
{
    internal abstract class SolutionItemTemplate<T> : SolutionItemTemplate
    {
        protected SolutionItemTemplate(T item)
        {
            Name = GetName(item);
        }

        protected abstract string GetNameOverride(T item);

        private string GetName(T item)
        {
            return GetNameOverride(item);
        }
    }

    internal abstract class SolutionItemTemplate
    {
        private const string SolutionFolderKind = "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}";

        private const string ProjectKind = "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}";

        public string Name { get; protected set; }

        public static SolutionItemTemplate Create(Project project)
        {
            switch (project.Kind)
            {
                case SolutionFolderKind:
                    return new SolutionFolderTemplate(project);
                case ProjectKind:
                    return new ProjectTemplate(project);
                default:
                    throw new ArgumentOutOfRangeException(nameof(project));
            }
        }
    }
}