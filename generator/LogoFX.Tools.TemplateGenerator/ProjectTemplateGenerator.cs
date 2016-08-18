using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
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

        public async Task GenerateAsync(string destinationFolder)
        {
            var projectFolder = Path.Combine(destinationFolder, _projectTemplateInfo.Name);
            Directory.CreateDirectory(projectFolder);

            var newProjectFileName = await CopyProjectToTemplateAsync(projectFolder);
            CreateDefinitions(projectFolder, newProjectFileName);
        }

        private async Task<string> CopyProjectToTemplateAsync(string projectFolder)
        {
            var from = Path.GetDirectoryName(_projectTemplateInfo.FileName);

            var newProjectName = Path.GetFileName(_projectTemplateInfo.FileName);
            Debug.Assert(newProjectName != null, "newProjectName != null");
            if (newProjectName.Length > 12)
            {
                newProjectName = "MyProject.csproj";
            }

                var newAbsolutePath = Path.Combine(projectFolder, newProjectName);
            File.Copy(_projectTemplateInfo.FileName, newAbsolutePath);

            Project project = new Project(newAbsolutePath);

            var x = project.GetProperty("ProjectGuid");
            x.UnevaluatedValue = "{$guid1$}";
            x = project.GetProperty("RootNamespace");
            var rootNamespace = x.EvaluatedValue;
            x.UnevaluatedValue = "$safeprojectname$";
            x = project.GetProperty("AssemblyName");
            x.UnevaluatedValue = "$safeprojectname$";

            foreach (var item in project.Items)
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
                        newFileName = CopyProjectItem(item, from, projectFolder);
                        break;
                }

                if (string.IsNullOrWhiteSpace(newFileName))
                {
                    continue;
                }

                ProjectItemTemplateGenerator fileGenerator = null;

                switch (item.ItemType)
                {
                    case "Compile":
                        fileGenerator = new CSFileGenerator(newFileName, rootNamespace, _solutionTemplateInfo);
                        break;
                    case "Page":
                    case "ApplicationDefinition":
                        fileGenerator = new XamlFileGenerator(newFileName, rootNamespace, _solutionTemplateInfo);
                        break;
                }

                fileGenerator?.Process();
            }

            project.Save();

            return newProjectName;
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
            var projectCollection = new XElement(s_ns + "Project",
                new XAttribute("TargetFileName", SafeRootProjectName(_projectTemplateInfo) + ".csproj"),
                new XAttribute("File", newProjectFileName),
                new XAttribute("ReplaceParameters", true));

            XDocument doc = new XDocument(
                new XElement(s_ns + "VSTemplate",
                    new XAttribute("Version", "3.0.0"),
                    new XAttribute("Type", "Project"),
                    new XElement(s_ns + "TemplateData",
                        new XElement(s_ns + "Name", _projectTemplateInfo.NameWithoutRoot),
                        new XElement(s_ns + "Description", ""),
                        new XElement(s_ns + "DefaultName", SafeRootProjectName(_projectTemplateInfo)),
                        new XElement(s_ns + "ProjectType", "CSharp"),
                        new XElement(s_ns + "ProjectSubType", ""),
                        new XElement(s_ns + "SortOrder", 1000),
                        new XElement(s_ns + "CreateNewFolder", true),
                        new XElement(s_ns + "ProvideDefaultName", true),
                        new XElement(s_ns + "LocationField", "Enabled"),
                        new XElement(s_ns + "EnableLocationBrowseButton", true),
                        new XElement(s_ns + "NumberOfParentCategoriesToRollUp", 1),
                        new XElement(s_ns + "Icon", "")),
                    new XElement(s_ns + "TemplateContent",
                        projectCollection),
                    new XElement(s_ns + "WizardExtension",
                        new XElement(s_ns + "Assembly", "LogoFX.Tools.Templates.Wizard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"),
                        new XElement(s_ns + "FullClassName", "LogoFX.Tools.Templates.Wizard.ProjectWizard")
                        )));

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;

            var templateFile = Path.Combine(projectFolder, "MyTemplate.vstemplate");

            using (XmlWriter xw = XmlWriter.Create(templateFile, settings))
            {
                doc.Save(xw);
            }
        }
    }
}