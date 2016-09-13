using System.Collections.Generic;
using System.Linq;
using EnvDTE;

namespace LogoFX.Tools.Templates.Wizard.Model
{
    internal abstract class SolutionItemCollectionTemplate<T> : SolutionItemTemplate<T>
    {
        private IEnumerable<SolutionItemTemplate> _items;

        protected SolutionItemCollectionTemplate(T item)
            : base(item)
        {
            CreateItems(item);
        }

        public IEnumerable<SolutionItemTemplate> Items => _items;

        protected abstract IEnumerable<Project> GetProjects(T item);

        private void CreateItems(T item)
        {
            _items = GetProjects(item).Select(Create).ToList();
        }
    }
}