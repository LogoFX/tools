using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE100;
using EnvDTE80;
using Microsoft.Build.Construction;
using Microsoft.VisualStudio.TemplateWizard;

namespace LogoFX.Tools.Templates.Wizard
{
    public sealed class SolutionWizard : IWizard
    {
        private const string Title = "New LogoFX WPF Samples Application";

        private const string SolutionFolderKind = "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}";

        private const string DebugWithFakeKey = "$debugwithfake$";

        private const string DebugWithFakeCondition =
            @"  <PropertyGroup Condition=""'$(Configuration)|$(Platform)' == 'DebugWithFake|AnyCPU'"">
    <DebugSymbols>true</DebugSymbols>
    <OutputPath>..\Bin\DebugWithFake\</OutputPath>
    <DefineConstants>DEBUG;TRACE</DefineConstants>
    <DebugType>full</DebugType>
    <PlatformTarget>AnyCPU</PlatformTarget>
    <ErrorReport>prompt</ErrorReport>
    <CodeAnalysisRuleSet>MinimumRecommendedRules.ruleset</CodeAnalysisRuleSet>
  </PropertyGroup>";

        private readonly TemplateBuilder.SolutionWizard _solutionWizard = 
            new TemplateBuilder.SolutionWizard();

        private readonly WizardData _wizardData = new WizardData();

        private Solution4 _solution;

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            _solutionWizard.RunStarted(automationObject, replacementsDictionary, runKind, customParams);

            if (runKind != WizardRunKind.AsMultiProject)
            {
                return;
            }

            DTE2 dtE2 = automationObject as DTE2;
            if (dtE2 != null)
            {
                _solution = (Solution4) dtE2.Solution;
            }

            var projectName = replacementsDictionary["$projectname$"];
            var form = new UserInputForm
            {
                Text = $"{Title} - {projectName}"
            };

            form.DataToScreen(_wizardData);
            var res = form.ShowDialog() == DialogResult.OK;
            form.ScreenToData(_wizardData);
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return _solutionWizard.ShouldAddProjectItem(filePath);
        }

        public void RunFinished()
        {
            _solutionWizard.RunFinished();
        }

        public void BeforeOpeningFile(ProjectItem projectItem)
        {
            _solutionWizard.BeforeOpeningFile(projectItem);
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
            _solutionWizard.ProjectItemFinishedGenerating(projectItem);
        }

        public void ProjectFinishedGenerating(Project project)
        {
            if (!_wizardData.Tests)
            {
                var testSolutionFolder = _solution.Projects
                    .OfType<Project>()
                    .FirstOrDefault(p => p.Name == "Tests");
                if (testSolutionFolder != null)
                {
                    _solution.Remove(testSolutionFolder);
                }
            }

            if (!_wizardData.FakeData)
            {
                var projects = GetProjects(true);
                foreach (var p in projects.Where(p => p.Name.Contains(".Fake.")))
                {
                    _solution.Remove(p);
                }
            }

            if (!_wizardData.FakeData || !_wizardData.Tests)
            {
                RemoveConditions();
            }

            _solutionWizard.ProjectFinishedGenerating(project);
        }

        private void RemoveConditions()
        {
            var projects = GetProjects();
            foreach (var project in projects)
            {
                RemoveConditions(project);
            }
        }

        private void RemoveConditions(Project project)
        {
            var projectFilePath = project.FileName;
            var solutionFilePath = project.DTE.Solution.FileName;

            if (!string.Equals(Path.GetExtension(projectFilePath), ".csproj"))
            {
                return;
            }

            var buildProject = new Microsoft.Build.Evaluation.Project(projectFilePath);
            var allGroups = buildProject.Xml.PropertyGroups;

            var toRemove = new List<ProjectPropertyGroupElement>();

            if (!_wizardData.FakeData)
            {
                toRemove.AddRange(allGroups.Where(x => x.Condition.Contains("Fake")));
            }

            if (!_wizardData.Tests)
            {
                toRemove.AddRange(allGroups.Where(x => x.Condition.Contains("Tests")));
            }

            if (toRemove.Count > 0)
            {
                foreach (var pg in toRemove.Distinct())
                {
                    buildProject.Xml.RemoveChild(pg);
                }
                buildProject.Save();
            }
        }

        private IList<Project> GetProjects(bool allowSolutionFolders = false)
        {
            Projects projects = _solution.Projects;
            List<Project> projectList = new List<Project>();
            foreach (object obj in projects)
            {
                Project solutionFolder = obj as Project;
                AddProjectsToList(solutionFolder, projectList, allowSolutionFolders);
            }
            return projectList;
        }

        private IEnumerable<Project> GetSolutionFolderProjects(Project solutionFolder, bool allowSolutionFolders)
        {
            List<Project> projectList = new List<Project>();
            for (int index = 1; index <= solutionFolder.ProjectItems.Count; ++index)
            {
                Project subProject = solutionFolder.ProjectItems.Item(index).SubProject;
                AddProjectsToList(subProject, projectList, allowSolutionFolders);
            }
            return projectList;
        }

        private void AddProjectsToList(Project rootProject, List<Project> projectList, bool allowSolutionFolders)
        {
            if (rootProject != null)
            {
                if (rootProject.Kind == SolutionFolderKind)
                {
                    projectList.AddRange(GetSolutionFolderProjects(rootProject, allowSolutionFolders));
                    if (allowSolutionFolders)
                    {
                        projectList.Add(rootProject);
                    }
                }
                else
                {
                    projectList.Add(rootProject);
                }
            }
        }
    }
}