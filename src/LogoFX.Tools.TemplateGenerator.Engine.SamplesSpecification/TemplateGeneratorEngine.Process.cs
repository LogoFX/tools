using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LogoFX.Tools.TemplateGenerator.Engine.Shared;

namespace LogoFX.Tools.TemplateGenerator.Engine.SamplesSpecification
{
    internal sealed partial class TemplateGeneratorEngine
    {
        private string GetFileContent(string fileName)
        {
            return File.ReadAllText(fileName);
        }

        private void SaveFileContent(string fileName, string content)
        {
            File.WriteAllText(fileName, content);
        }

        private string SafeRootProjectName(ProjectTemplateInfo projectTemplateInfo)
        {
            return $"$saferootprojectname$.{projectTemplateInfo.NameWithoutRoot}";
        }

        private Task ProcessCs(string fileName, string rootNamespace, IEnumerable<ProjectTemplateInfo> projects)
        {
            return Task.Run(() =>
            {
                var content = GetFileContent(fileName);
                content = content.Replace(rootNamespace, "$safeprojectname$");

                foreach (var project in projects)
                {
                    if (!content.Contains(project.Name))
                    {
                        continue;
                    }

                    content = content.Replace(project.Name, SafeRootProjectName(project));
                }

                foreach (var rn in _rootNamespaces.OrderByDescending(x => x).Distinct())
                {
                    if (!content.Contains(rn))
                    {
                        continue;
                    }

                    content = content.Replace(rootNamespace, "$saferootprojectname$");
                }

                SaveFileContent(fileName, content);
            });
        }
        
        private Task ProcessXaml(string fileName, string rootNamespace, IEnumerable<ProjectTemplateInfo> projects)
        {
            return Task.Run(() =>
            {
                var content = GetFileContent(fileName);
                content = content.Replace(rootNamespace, "$safeprojectname$");

                foreach (var project in projects)
                {
                    if (!content.Contains(project.Name))
                    {
                        continue;
                    }

                    content = content.Replace(project.Name, SafeRootProjectName(project));
                }
                SaveFileContent(fileName, content);
            });
        }
    }
}