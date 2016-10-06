using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using LogoFX.Tools.TemplateGenerator.Contracts;

namespace LogoFX.Tools.TemplateGenerator
{
    public sealed class SolutionTemplateGenerator : GeneratorBase
    {
        public async Task GenerateAsync(string destinationFolder, WizardConfiguration wizardConfiguration)
        {
            var definitionsGenerator = new DefinitionsGenerator(destinationFolder);

            CleanDestination(destinationFolder);
            definitionsGenerator.CreateDefinitions(wizardConfiguration);

            await CreateWizardSolutionFileAsync(destinationFolder, wizardConfiguration);
            CreatePrepropcess(destinationFolder);

            foreach (var solution in wizardConfiguration.Solutions)
            {
                foreach (var projectTemplateInfo in solution.SolutionTemplateInfo.GetProjectsPlain())
                {
                    var projectGenerator = new ProjectTemplateGenerator(projectTemplateInfo, solution.SolutionTemplateInfo);
                    await projectGenerator.GenerateAsync();
                }
            }
        }

        private async Task CreateWizardSolutionFileAsync(string destinationFolder, WizardConfiguration wizardConfiguration)
        {
            //TODO: Add code here
            var wizardDataGenrator = new WizardDataGenerator(wizardConfiguration);
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

            Directory.Delete(destinationFolder, true);
        }
    }
}