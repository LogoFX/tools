using System.Collections.Generic;
using LogoFX.Tools.TemplateGenerator.Contracts;

namespace LogoFX.Tools.TemplateGenerator
{
    internal sealed class XamlFileGenerator : ProjectItemTemplateGenerator
    {
        private readonly string _rootNamespace;
        private readonly IEnumerable<IProjectTemplateInfo> _projects;

        public XamlFileGenerator(string fileName, string rootNamespace, IEnumerable<IProjectTemplateInfo> projects)
            : base(fileName)
        {
            _rootNamespace = rootNamespace;
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

            SaveFileContent(content);
        }
    }
}