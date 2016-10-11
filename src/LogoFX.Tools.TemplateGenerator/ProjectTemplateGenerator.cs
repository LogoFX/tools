using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using LogoFX.Tools.TemplateGenerator.Contracts;
using Microsoft.Build.Evaluation;

namespace LogoFX.Tools.TemplateGenerator
{
    internal sealed class ProjectTemplateGenerator : GeneratorBase
    {
        private readonly IProjectTemplateInfo _projectTemplateInfo;
        private readonly ISolutionTemplateInfo _solutionTemplateInfo;

        public ProjectTemplateGenerator(IProjectTemplateInfo projectTemplateInfo, ISolutionTemplateInfo solutionTemplateInfo)
        {
            _projectTemplateInfo = projectTemplateInfo;
            _solutionTemplateInfo = solutionTemplateInfo;
        }

        public async Task GenerateAsync()
        {
            var projectFolder = Path.GetDirectoryName(_projectTemplateInfo.DestinationFileName);
            Directory.CreateDirectory(projectFolder);

            var newProjectFileName = await CopyProjectToTemplateAsync(projectFolder);
            //CreateDefinitions(projectFolder, newProjectFileName);
        }

        private async Task<string> CopyProjectToTemplateAsync(string projectFolder)
        {
            var from = Path.GetDirectoryName(_projectTemplateInfo.FileName);

            if (File.Exists(_projectTemplateInfo.DestinationFileName))
            {
                Debugger.Break();
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
                        newFileName = CopyProjectItem(item, from, projectFolder);
                        if (string.IsNullOrEmpty(newFileName))
                        {
                            var success = project.RemoveItem(item);
                        }
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
                        fileGenerator = new CSFileGenerator(newFileName, rootNamespace, _solutionTemplateInfo);
                        break;
                    case ".xaml":
                        fileGenerator = new XamlFileGenerator(newFileName, rootNamespace, _solutionTemplateInfo);
                        break;
                }

                fileGenerator?.Process();
            }

            project.Save();

            return _projectTemplateInfo.DestinationFileName;
        }

        private string CopyProjectItem(ProjectItem item, string from, string to)
        {
            var oldFileName = Path.Combine(from, item.EvaluatedInclude);

            if (!File.Exists(oldFileName))
            {
                return null;
            }

            var newFileName = Path.Combine(to, item.EvaluatedInclude);
            var newFolder = Path.GetDirectoryName(newFileName);
            if (!Directory.Exists(newFolder))
            {
                Directory.CreateDirectory(newFolder);
            }
            File.Copy(oldFileName, newFileName);

            return newFileName;
        }

        private void FixReference(ProjectItem reference)
        {
            var include = reference.EvaluatedInclude;
            var rootName = GetRootName(include);
            reference.UnevaluatedInclude = include.Replace(rootName, "$saferootprojectname$");
            var name = reference.Metadata.SingleOrDefault(x => x.Name == "Name");
            if (name != null)
            {
                name.UnevaluatedValue = name.EvaluatedValue.Replace(rootName, "$saferootprojectname$");
            }
        }

        private void CreateDefinitions(string projectFolder, string newProjectFileName)
        {
            var projectCollection = new XElement(Ns + "Project",
                new XAttribute("TargetFileName", SafeRootProjectName(_projectTemplateInfo) + ".csproj"),
                new XAttribute("File", newProjectFileName),
                new XAttribute("ReplaceParameters", true));

            var doc = new XDocument(
                new XElement(Ns + "VSTemplate",
                    new XAttribute("Version", "3.0.0"),
                    new XAttribute("Type", "Project"),
                    new XElement(Ns + "TemplateData",
                        new XElement(Ns + "Name", _projectTemplateInfo.NameWithoutRoot),
                        new XElement(Ns + "Description", ""),
                        new XElement(Ns + "DefaultName", SafeRootProjectName(_projectTemplateInfo)),
                        new XElement(Ns + "ProjectType", "CSharp"),
                        new XElement(Ns + "ProjectSubType", ""),
                        new XElement(Ns + "SortOrder", 1000),
                        new XElement(Ns + "CreateNewFolder", true),
                        new XElement(Ns + "ProvideDefaultName", true),
                        new XElement(Ns + "LocationField", "Enabled"),
                        new XElement(Ns + "EnableLocationBrowseButton", true),
                        new XElement(Ns + "NumberOfParentCategoriesToRollUp", 1),
                        new XElement(Ns + "Icon", "")),
                    new XElement(Ns + "TemplateContent", projectCollection),
                    MakeWizardExtension(
                        "TemplateBuilder, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null",
                        "TemplateBuilder.ChildWizard")
                    ));

            XmlWriterSettings settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true
            };

            var templateFile = Path.Combine(projectFolder, "MyTemplate.vstemplate");

            using (XmlWriter xw = XmlWriter.Create(templateFile, settings))
            {
                doc.Save(xw);
            }
        }
    }
}