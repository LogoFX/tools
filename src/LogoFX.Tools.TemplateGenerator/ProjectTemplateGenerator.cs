using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LogoFX.Tools.TemplateGenerator.Contracts;
using Microsoft.Build.Evaluation;

namespace LogoFX.Tools.TemplateGenerator
{
    internal sealed class ProjectTemplateGenerator : GeneratorBase
    {
        private readonly IProjectTemplateInfo _projectTemplateInfo;
        private readonly IEnumerable<IProjectTemplateInfo> _projects;

        public ProjectTemplateGenerator(IProjectTemplateInfo projectTemplateInfo, IEnumerable<IProjectTemplateInfo> projects)
        {
            _projectTemplateInfo = projectTemplateInfo;
            _projects = projects;
        }

        public async Task GenerateAsync()
        {
            var projectFolder = Path.GetDirectoryName(_projectTemplateInfo.DestinationFileName);
            Directory.CreateDirectory(projectFolder);

            await CopyProjectToTemplateAsync(projectFolder);
        }

        private async Task CopyProjectToTemplateAsync(string projectFolder)
        {
            var from = Path.GetDirectoryName(_projectTemplateInfo.FileName);

            if (File.Exists(_projectTemplateInfo.DestinationFileName))
            {
                //Debugger.Break();
                return;
            }

            File.Copy(_projectTemplateInfo.FileName, _projectTemplateInfo.DestinationFileName);

            Project project = new Project(_projectTemplateInfo.DestinationFileName);

            var x = project.GetProperty("ProjectGuid");
            x.UnevaluatedValue = "{$guid1$}";
            x = project.GetProperty("RootNamespace");
            var rootNamespace = x.EvaluatedValue;
            x.UnevaluatedValue = "$safeprojectname$";
            x = project.GetProperty("AssemblyName");
            x.UnevaluatedValue = "$safeprojectname$";

            foreach (var item in project.Items.ToList())
            {
                string newFileName = null;

                switch (item.ItemType)
                {
                    case "ProjectReference":
                        FixReference(item);
                        break;
                    case "Compile":
                    case "None":
                    case "Page":
                    case "ApplicationDefinition":
                    case "EmbeddedResource":
                    case "Content":
                    case "AppxManifest":
                    case "Service":
                    case "SDKReference":
                    case "Resource":
                        newFileName = await CopyProjectItem(item, from, projectFolder);
                        break;
                }

                if (string.IsNullOrWhiteSpace(newFileName))
                {
                    continue;
                }

                ProjectItemTemplateGenerator fileGenerator = null;

                var ext = Path.GetExtension(newFileName);
                switch (ext)
                {
                    case ".cs":
                    case ".config":
                        fileGenerator = new CSFileGenerator(newFileName, rootNamespace, _projects);
                        break;
                    case ".xaml":
                        fileGenerator = new XamlFileGenerator(newFileName, rootNamespace, _projects);
                        break;
                }

                fileGenerator?.Process();
            }

            project.Save();
        }

        private async Task<string> CopyProjectItem(ProjectItem item, string from, string to)
        {
            var oldFileName = Path.Combine(from, item.EvaluatedInclude);

            if (!File.Exists(oldFileName))
            {
                return null;
            }

            var newFileName = Path.Combine(to, item.EvaluatedInclude);

            if (File.Exists(newFileName))
            {
                return newFileName;
            }

            var newFolder = Path.GetDirectoryName(newFileName);
            Debug.Assert(newFolder != null, "newFolder != null");
            if (!Directory.Exists(newFolder))
            {
                Directory.CreateDirectory(newFolder);
            }

            await Task.Run(() => { File.Copy(oldFileName, newFileName); });

            return newFileName;
        }

        private void FixReference(ProjectItem reference)
        {
            var include = reference.EvaluatedInclude;
            var fileName = Path.GetFileName(include);
            var dir = Path.GetFileNameWithoutExtension(fileName);
            var ddir = Path.GetDirectoryName(Path.GetDirectoryName(include));
            include = Path.Combine(ddir, Path.Combine(dir, fileName));
            var rootName = GetRootName(include);
            reference.UnevaluatedInclude = include.Replace(rootName, "$saferootprojectname$");
            var name = reference.Metadata.SingleOrDefault(x => x.Name == "Name");
            if (name != null)
            {
                name.UnevaluatedValue = name.EvaluatedValue.Replace(rootName, "$saferootprojectname$");
            }
        }
    }
}