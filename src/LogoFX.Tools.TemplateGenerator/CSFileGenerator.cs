using System.Collections.Generic;
using System.Linq;
using LogoFX.Tools.TemplateGenerator.Contracts;

namespace LogoFX.Tools.TemplateGenerator
{
    internal sealed class CsFileGenerator : ProjectItemTemplateGenerator
    {
        private readonly string _rootNamespace;
        private readonly IEnumerable<string> _rootNamespaces;
        private readonly IEnumerable<IProjectTemplateInfo> _projects;

        public CsFileGenerator(string fileName, string rootNamespace, IEnumerable<string> rootNamespaces, IEnumerable<IProjectTemplateInfo> projects) 
            : base(fileName)
        {
            _rootNamespace = rootNamespace;
            _rootNamespaces = rootNamespaces;
            _projects = projects;
        }

        protected override void ProcessInternal()
        {
            var content = GetFileContent();
            content = content.Replace(_rootNamespace, "$safeprojectname$");

            foreach (var project in _projects)
            {
                if (!content.Contains(project.Name))
                {
                    continue;
                }

                content = content.Replace(project.Name, SafeRootProjectName(project));
            }

            foreach (var rootNamespace in _rootNamespaces.OrderByDescending(x => x).Distinct())
            {
                if (!content.Contains(rootNamespace))
                {
                    continue;
                }

                content = content.Replace(rootNamespace, "$saferootprojectname$");
            }

            SaveFileContent(content);
        }
    }
}