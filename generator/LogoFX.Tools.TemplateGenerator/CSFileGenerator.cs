namespace LogoFX.Tools.TemplateGenerator
{
    internal sealed class CSFileGenerator : ProjectItemTemplateGenerator
    {
        private readonly string _rootNamespace;
        private readonly ISolutionTemplateInfo _solutionTemplateInfo;

        public CSFileGenerator(string fileName, string rootNamespace, ISolutionTemplateInfo solutionTemplateInfo) 
            : base(fileName)
        {
            _rootNamespace = rootNamespace;
            _solutionTemplateInfo = solutionTemplateInfo;
        }

        protected override void ProcessInternal()
        {
            var content = GetFileContent();
            content = content.Replace(_rootNamespace, "$safeprojectname$");

            foreach (var project in _solutionTemplateInfo.GetProjectsPlain())
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

    internal sealed class XamlFileGenerator : ProjectItemTemplateGenerator
    {
        private readonly string _rootNamespace;
        private readonly ISolutionTemplateInfo _solutionTemplateInfo;

        public XamlFileGenerator(string fileName, string rootNamespace, ISolutionTemplateInfo solutionTemplateInfo)
            : base(fileName)
        {
            _rootNamespace = rootNamespace;
            _solutionTemplateInfo = solutionTemplateInfo;
        }

        protected override void ProcessInternal()
        {
            var content = GetFileContent();
            content = content.Replace(_rootNamespace, "$safeprojectname$");

            foreach (var project in _solutionTemplateInfo.GetProjectsPlain())
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