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

            CreateWizardSolutionFile(wizardConfiguration);
            CreatePrepropcess(destinationFolder);

            var multiSolution = wizardConfiguration.Solutions.Count > 1;

            foreach (var solution in wizardConfiguration.Solutions)
            {
                string solutionFolder;
                if (multiSolution)
                {
                    solutionFolder = Path.Combine(destinationFolder, solution.Name);
                    if (!Directory.Exists(solutionFolder))
                    {
                        Directory.CreateDirectory(destinationFolder);
                    }
                }
                else
                {
                    solutionFolder = destinationFolder;
                }

                foreach (var projectTemplateInfo in solution.SolutionTemplateInfo.GetProjectsPlain())
                {
                    var projectGenerator = new ProjectTemplateGenerator(projectTemplateInfo, solution.SolutionTemplateInfo);
                    await projectGenerator.GenerateAsync(solutionFolder);
                }
            }
        }

        private void CreateWizardSolutionFile(WizardConfiguration wizardConfiguration)
        {
            //TODO: Add code here
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