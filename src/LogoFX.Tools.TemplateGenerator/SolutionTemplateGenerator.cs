using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using LogoFX.Tools.TemplateGenerator.Contracts;

namespace LogoFX.Tools.TemplateGenerator
{
    public sealed class SolutionTemplateGenerator : GeneratorBase
    {
        private const string DefinitionsFolderName = "Definitions";

        private readonly WizardConfiguration _wizardConfiguration;

        public SolutionTemplateGenerator(WizardConfiguration wizardConfiguration)
        {
            _wizardConfiguration = wizardConfiguration;
        }

        public async Task GenerateAsync(string destinationFolder)
        {
            var definitionsGenerator = new DefinitionsGenerator(destinationFolder);

            CleanDestination(destinationFolder);
            definitionsGenerator.CreateDefinitions(_wizardConfiguration);

            CreatePrepropcess(destinationFolder);

            var folderDic = new Dictionary<string, string>();
            foreach (var solution in _wizardConfiguration.Solutions)
            {
                foreach (var variant in solution.SolutionVariants)
                {
                    var projects = variant.SolutionTemplateInfo.GetProjectsPlain().ToList();

                    foreach (var projectTemplateInfo in projects)
                    {
                        var folderName = Path.GetDirectoryName(projectTemplateInfo.FileName);
                        folderName = Path.GetFileName(folderName);
                        var destinationFileName = CreateNewFileName(folderName, solution.Name, destinationFolder, folderDic);
                        projectTemplateInfo.SetDestinationFileName(destinationFileName);
                        if (File.Exists(destinationFileName))
                        {
                            continue;
                        }
                        var projectGenerator = new ProjectTemplateGenerator(projectTemplateInfo, variant.SolutionTemplateInfo.RootNamespaces, projects);
                        await projectGenerator.GenerateAsync();
                    }
                }
            }

            await CreateWizardSolutionFileAsync(destinationFolder, _wizardConfiguration);
        }

        private string CreateNewFileName(string projectName, string solutionName, string destinationFolder, Dictionary<string, string> folderDic)
        {
            var solutionFolder = _wizardConfiguration.Solutions.Count > 1
                ? Path.Combine(destinationFolder, solutionName)
                : destinationFolder;

            var newProjectName = projectName;
            Debug.Assert(newProjectName != null, "newProjectName != null");
            if (newProjectName.Length > 12)
            {
                newProjectName = "MyProject.csproj";
            }

            if (!folderDic.TryGetValue(projectName, out var shortProjectName))
            {
                shortProjectName = $"Proj{folderDic.Count + 1:D3}";
                folderDic.Add(projectName, shortProjectName);
            }

            var result = Path.Combine(solutionFolder, shortProjectName);
            result = Path.Combine(result, newProjectName);

            return result;
        }

        private async Task CreateWizardSolutionFileAsync(string destinationFolder, WizardConfiguration wizardConfiguration)
        {
            var wizardDataGenrator = new WizardDataGenerator(wizardConfiguration);

            destinationFolder = Path.Combine(destinationFolder, DefinitionsFolderName);
            await wizardDataGenrator.GenerateAndSaveAsync(destinationFolder);
        }

        private void CreatePrepropcess(string destinationFolder)
        {
            var doc = new XDocument(
                new XElement("Preprocess",
                    new XElement("TemplateInfo",
                        new XAttribute("Path", "CSharp\\LogoFX")),
                    new XElement("Replacements",
                        new XAttribute("Include", "*.*"),
                        new XAttribute("Exclude", "*.vstemplate;*.csproj;*.fsproj;*.vbproj;*.jpg;*.png;*.ico;_preprocess.xml;_project.vstemplate.xml"))));

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            destinationFolder = Path.Combine(destinationFolder, DefinitionsFolderName);
            var definitionFile = Path.Combine(destinationFolder, "_preprocess.xml");

            using (XmlWriter xw = XmlWriter.Create(definitionFile, settings))
            {
                doc.Save(xw);
            }
        }

        private void CleanDestination(string destinationFolder)
        {
            if (!Directory.Exists(destinationFolder))
            {
                return;
            }

            var dirInfo = new DirectoryInfo(destinationFolder);
            foreach (var subDir in dirInfo.EnumerateDirectories())
            {
                subDir.Delete(true);
            }
        }
    }
}