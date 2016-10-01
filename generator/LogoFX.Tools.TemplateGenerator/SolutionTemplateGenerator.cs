using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace LogoFX.Tools.TemplateGenerator
{
    public sealed class SolutionTemplateGenerator : GeneratorBase
    {
        public async Task GenerateAsync(string destinationFolder, WizardConfiguration wizardConfiguration)
        {
            if (!Directory.Exists(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);
            }

            var definitionsGenerator = new DefinitionsGenerator(destinationFolder);

            CleanDestination(destinationFolder, wizardConfiguration);
            definitionsGenerator.CreateDefinitions(new TemplateDataInfo
            {
                DefaultName = wizardConfiguration.DefaultName,
                Description = wizardConfiguration.Description,
                Name = wizardConfiguration.Name,
                WizardClassName = Path.GetFileNameWithoutExtension(wizardConfiguration.CodeFileName)
            }, solutionTemplateInfo);

            CreateWizardSolutionFile(wizardConfiguration);
            CreatePrepropcess(destinationFolder);

            var solutionFolder = destinationFolder;
            if (_isMultisolution)
            {
                solutionFolder = Path.Combine(destinationFolder, _currentName);
                if (!Directory.Exists(solutionFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }
            }

            foreach (var projectTemplateInfo in solutionTemplateInfo.GetProjectsPlain())
            {
                var projectGenerator = new ProjectTemplateGenerator(projectTemplateInfo, solutionTemplateInfo);
                await projectGenerator.GenerateAsync(solutionFolder);
            }
        }

        private string BoolToString(bool value)
        {
            return value ? "true" : "false";
        }

        private void Generate_WizardConfiguration_Solutions(StringBuilder sb, IEnumerable<SolutionInfo> solutions)
        {
            sb.AppendLine("                Solutions = new List<SolutionInfo>");
            sb.AppendLine("                {");
            foreach (var solution in solutions)
            {
                sb.AppendLine("                    new SolutionInfo");
                sb.AppendLine("                    {");
                sb.AppendLine($"                        Name = \"{solution.Name}\",");
                sb.AppendLine($"                        Caption = \"{solution.Caption}\",");
                sb.AppendLine($"                        IconName = \"{solution.IconName}\",");
                sb.AppendLine("                    },");
            }
            sb.AppendLine("                },");
        }

        private void Generate_WizardConfigurtion(StringBuilder sb, WizardConfiguration wizardConfiguration)
        {
            sb.AppendLine("            return new WizardConfiguration");
            sb.AppendLine("            {");
            sb.AppendLine($"                FakeOption={BoolToString(wizardConfiguration.FakeOption)},");
            sb.AppendLine($"                TestOption={BoolToString(wizardConfiguration.TestOption)},");
            if (wizardConfiguration.Solutions.Count > 0)
            {
                Generate_WizardConfiguration_Solutions(sb, wizardConfiguration.Solutions);
            }
            sb.AppendLine("            };");
        }

        private void Generate_GetWizardConfiguration(StringBuilder sb, WizardConfiguration wizardConfiguration)
        {
            sb.AppendLine("        protected override WizardConfiguration GetWizardConfiguration()");
            sb.AppendLine("        {");
            if (wizardConfiguration.Solutions.Count > 1)
            {
                Generate_WizardConfigurtion(sb, wizardConfiguration);
            }
            else
            {
                sb.AppendLine("            return null;");
            }
            sb.AppendLine("        }");
        }

        private void CreateWizardSolutionFile(WizardConfiguration wizardConfiguration)
        {
            var fileName = wizardConfiguration.CodeFileName;
            var name = Path.GetFileNameWithoutExtension(fileName);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using LogoFX.Tools.TemplateGenerator;");
            sb.AppendLine();
            sb.AppendLine("namespace LogoFX.Tools.Templates.Wizard");
            sb.AppendLine("{");
            sb.AppendLine($"    public sealed class {name} : SolutionWizard");
            sb.AppendLine("    {");

            sb.AppendLine("        protected override string GetTitle()");
            sb.AppendLine("        {");
            sb.AppendLine($"            return \"New {wizardConfiguration.Name}\";");
            sb.AppendLine("        }");
            sb.AppendLine();

            Generate_GetWizardConfiguration(sb, wizardConfiguration);

            sb.AppendLine("    }");
            sb.AppendLine("}");

            File.WriteAllText(fileName, sb.ToString());
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

        private void CleanDestination(string destinationFolder, WizardConfiguration wizardConfiguration)
        {
            if (!Directory.Exists(destinationFolder))
            {
                return;
            }

            if (!_isMultisolution)
            {
                Directory.Delete(destinationFolder, true);
                return;
            }

            var dir = new DirectoryInfo(destinationFolder);
            foreach (var info in dir.EnumerateFileSystemInfos())
            {
                if (info is FileInfo)
                {
                    info.Delete();
                    continue;
                }

                if (Utils.FileNamesAreEqual(_currentName, info.Name) ||
                    wizardConfiguration.Solutions.All(x => !Utils.FileNamesAreEqual(x.Name, info.Name)))
                {
                    ((DirectoryInfo) info).Delete(true);
                }
            }
        }
    }
}