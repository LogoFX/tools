using System.IO;

namespace LogoFX.Tools.TemplateGenerator
{
    internal abstract class ProjectItemTemplateGenerator : GeneratorBase
    {
        private readonly string _fileName;

        protected ProjectItemTemplateGenerator(string fileName)
        {
            _fileName = fileName;
        }

        public void Process()
        {
            ProcessInternal();
        }

        protected abstract void ProcessInternal();

        protected string GetFileContent()
        {
            return File.ReadAllText(_fileName);
        }

        protected void SaveFileContent(string content)
        {
            File.WriteAllText(_fileName, content);
        }
    }
}