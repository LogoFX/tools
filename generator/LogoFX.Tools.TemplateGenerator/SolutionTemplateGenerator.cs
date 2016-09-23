using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using LogoFX.Tools.TemplateGenerator.Contracts;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using Project = Microsoft.Build.Evaluation.Project;

namespace LogoFX.Tools.TemplateGenerator
{
    public sealed class SolutionTemplateGenerator : GeneratorBase
    {
        private SolutionTemplateInfo _solutionTemplateInfo;

        private readonly string _solutionFileName;
        private readonly bool _isMultisolution;
        private readonly string _currentName;

        public SolutionTemplateGenerator(string solutionFileName, bool isMultisolution)
        {
            _solutionFileName = solutionFileName;
            _isMultisolution = isMultisolution;
            _currentName = Path.GetFileNameWithoutExtension(solutionFileName);
        }

        public async Task<ISolutionTemplateInfo> GetInfoAsync()
        {
            if (_solutionTemplateInfo == null)
            {
                _solutionTemplateInfo = await GenerateTemplateInfoAsync();
            }

            return _solutionTemplateInfo;
        }

        public async Task GenerateAsync(
            string destinationFolder, 
            ISolutionTemplateInfo solutionTemplateInfo,
            WizardConfiguration wizardConfiguration)
        {
            if (solutionTemplateInfo == null)
            {
                solutionTemplateInfo = await GetInfoAsync();
            }

            if (!Directory.Exists(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);
            }

            var definitionsGenerator = new DefinitionsGenerator(destinationFolder, _isMultisolution ? _currentName : null);

            if (_isMultisolution)
            {
                solutionTemplateInfo = RebuildForMultisolution(definitionsGenerator, solutionTemplateInfo);
            }

            CleanDestination(destinationFolder, wizardConfiguration);
            definitionsGenerator.CreateDefinitions(new TemplateDataInfo
            {
                DefaultName = wizardConfiguration.DefaultName,
                Description = wizardConfiguration.Description,
                Name = wizardConfiguration.Name,
                WizardClassName = _isMultisolution
                    ? Path.GetFileNameWithoutExtension(wizardConfiguration.CodeFileName)
                    : "SolutionWizard"
            }, solutionTemplateInfo);

            if (_isMultisolution)
            {
                CreateWizardSolutionFile(wizardConfiguration);
            }
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

        private ISolutionTemplateInfo RebuildForMultisolution(DefinitionsGenerator definitionsGenerator, ISolutionTemplateInfo solutionTemplateInfo)
        {
            var newSolutionInfo = new SolutionTemplateInfo();
            foreach (var rn in solutionTemplateInfo.RootNamespaces)
            {
                newSolutionInfo.RootNamespaces.Add(rn);
            }
            var solutionFolder = new SolutionFolderTemplateInfo(Guid.Empty, _currentName);
            foreach (var item in solutionTemplateInfo.Items)
            {
                solutionFolder.Items.Add((SolutionItemTemplateInfo) item);
            }
            newSolutionInfo.Items.Add(solutionFolder);

            var oldSolutionInfo = definitionsGenerator.LoadDefinitions();

            if (oldSolutionInfo != null)
            {
                foreach (var oldSolutionItem in oldSolutionInfo.Items)
                {
                    if (oldSolutionItem.Name != _currentName)
                    {
                        newSolutionInfo.Items.Add((SolutionItemTemplateInfo) oldSolutionItem);
                    }
                }
            }

            return newSolutionInfo;
        }

        private string BoolToString(bool value)
        {
            return value ? "true" : "false";
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
            sb.AppendLine("        protected override WizardConfiguration GetWizardConfiguration()");
            sb.AppendLine("        {");
            sb.AppendLine("            return new WizardConfiguration");
            sb.AppendLine("            {");
            sb.AppendLine($"                FakeOption={BoolToString(wizardConfiguration.FakeOption)},");
            sb.AppendLine($"                TestOption={BoolToString(wizardConfiguration.TestOption)},");
            if (wizardConfiguration.Solutions.Count > 0)
            {
                sb.AppendLine("                Solutions = new List<SolutionInfo>");
                sb.AppendLine("                {");
                foreach (var solution in wizardConfiguration.Solutions)
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
            sb.AppendLine("            };");
            sb.AppendLine("        }");
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

                if (FileNameEquals(_currentName, info.Name) ||
                    wizardConfiguration.Solutions.All(x => !FileNameEquals(x.Name, info.Name)))
                {
                    ((DirectoryInfo) info).Delete(true);
                }
            }
        }

        private bool FileNameEquals(string fileName1, string fileName2)
        {
            return string.Compare(fileName1, fileName2, StringComparison.OrdinalIgnoreCase) == 0;
        }

        private async Task<SolutionTemplateInfo> GenerateTemplateInfoAsync()
        {
            SolutionFile solution = SolutionFile.Parse(_solutionFileName);
            SolutionTemplateInfo solutionTemplateInfo = new SolutionTemplateInfo();

            var folders= new Dictionary<Guid, SolutionFolderTemplateInfo>();
            folders.Add(Guid.Empty, solutionTemplateInfo);

            foreach (var proj in solution.ProjectsInOrder)
            {
                await CreateSolutionItemTemplateInfoAsync(solution, solutionTemplateInfo, proj, folders);
            }

            ProjectCollection.GlobalProjectCollection.UnloadAllProjects();

            return solutionTemplateInfo;
        }

        private async Task<SolutionItemTemplateInfo> CreateSolutionItemTemplateInfoAsync(SolutionFile solution,
            SolutionTemplateInfo solutionTemplateInfo,
            ProjectInSolution proj, 
            IDictionary<Guid, SolutionFolderTemplateInfo> folders)
        {
            var id = Guid.Parse(proj.ProjectGuid);
            var parentId = proj.ParentProjectGuid == null ? Guid.Empty : Guid.Parse(proj.ParentProjectGuid);

            SolutionFolderTemplateInfo folder;
            if (!folders.TryGetValue(parentId, out folder))
            {
                ProjectInSolution parentProj;
                if (solution.ProjectsByGuid.TryGetValue(proj.ParentProjectGuid, out parentProj))
                {
                    folder = (SolutionFolderTemplateInfo) await CreateSolutionItemTemplateInfoAsync(solution, solutionTemplateInfo, parentProj, folders);
                }
            }

            Debug.Assert(folder != null);

            SolutionItemTemplateInfo result;

            switch (proj.ProjectType)
            {
                case SolutionProjectType.KnownToBeMSBuildFormat:
                    Project project = new Project(proj.AbsolutePath);
                    var rootNamespace = project.Properties.Single(x => x.Name == "RootNamespace").EvaluatedValue;
                    var rootName = AddRootName(rootNamespace, solutionTemplateInfo);
                    result = new ProjectTemplateInfo(id, proj.ProjectName)
                    {
                        NameWithoutRoot = proj.ProjectName.Substring(rootName.Length + 1),
                        FileName = proj.AbsolutePath
                    };
                    break;
                case SolutionProjectType.SolutionFolder:
                    if (folders.ContainsKey(id))
                    {
                        return folders[id];
                    }
                    result = new SolutionFolderTemplateInfo(id, proj.ProjectName);
                    folders.Add(id, (SolutionFolderTemplateInfo) result);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            folder.Items.Add(result);

            return result;
        }

        private string AddRootName(string projectName, SolutionTemplateInfo solutionTemplateInfo)
        {
            var rootName = GetRootName(projectName);

            if (!solutionTemplateInfo.RootNamespaces.Contains(rootName))
            {
                solutionTemplateInfo.RootNamespaces.Add(rootName);
            }

            return rootName;
        }
    }
}