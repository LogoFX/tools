﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using EnvDTE;
using EnvDTE100;
using EnvDTE80;
using LogoFX.Tools.Common.Model;
using LogoFX.Tools.Templates.Wizard.Model;
using LogoFX.Tools.Templates.Wizard.ViewModel;
using LogoFX.Tools.Templates.Wizard.Views;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using Microsoft.VisualStudio.TemplateWizard;
using Project = EnvDTE.Project;
using ProjectItem = Microsoft.Build.Evaluation.ProjectItem;

namespace LogoFX.Tools.Templates.Wizard
{
    public sealed class SolutionWizard : SolutionWizardBase
    {
        #region Fields

        private Dictionary<string, string> _replacementsDictionary;
        private WizardDataViewModel _wizardDataViewModel;
        private string _tmpFolder;

        #endregion

        #region Private Members

        private void RemoveCondition(Microsoft.Build.Evaluation.Project buildProject, SolutionDataViewModel solutionData)
        {
            var allGroups = buildProject.Xml.PropertyGroups;

            var toRemove = new List<ProjectPropertyGroupElement>();

            if (!solutionData.CreateFakes)
            {
                toRemove.AddRange(allGroups.Where(x => x.Condition.Contains("Fake")));
            }

            if (!solutionData.CreateFakes)
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

        private void ModifyProject(string projectFilePath, SolutionDataViewModel solutionData)
        {
            if (!string.Equals(Path.GetExtension(projectFilePath), ".csproj"))
            {
                return;
            }

            var buildProject = new Microsoft.Build.Evaluation.Project(projectFilePath);

            if (solutionData.MustRemoveConditions)
            {
                RemoveCondition(buildProject, solutionData);
            }

            if (!solutionData.CreateSamples && Path.GetFileNameWithoutExtension(projectFilePath).EndsWith("Shell"))
            {
                RemoveSamples(buildProject, solutionData);
            }

            ProjectCollection.GlobalProjectCollection.UnloadProject(buildProject);
            //System.Threading.Thread.Sleep(500);
        }

        private void RemoveSamples(Microsoft.Build.Evaluation.Project buildProject, SolutionDataViewModel solutionData)
        {
            var items = buildProject.Items.ToArray();
            var toRemove = new List<ProjectItem>();
            foreach (var item in items)
            {
                var name = item.EvaluatedInclude;
                var index = name.IndexOf(Path.DirectorySeparatorChar);
                if (index < 0)
                {
                    continue;
                }
                var folder = name.Substring(0, index);

                switch (folder)
                {
                    case "Views":
                    case "ViewModels":
                    case "Resources":
                        var itemName = Path.GetFileNameWithoutExtension(name);
                        if (itemName == "ShellViewModel")
                        {
                            var body = CreateEmptyShell(buildProject);
                            var itemFileName = Path.GetDirectoryName(item.Xml.IncludeLocation.File);
                            itemFileName = Path.Combine(itemFileName, name);
                            File.WriteAllText(itemFileName, body);
                            continue;
                        }
                        toRemove.Add(item);
                        break;
                }
            }

            if (toRemove.Count > 0)
            {
                buildProject.RemoveItems(toRemove);
                buildProject.Save();
            }
        }

        private string CreateEmptyShell(Microsoft.Build.Evaluation.Project buildProject)
        {
            var rootNamespace = buildProject.Properties.Single(x => x.Name == "RootNamespace").EvaluatedValue;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("using Caliburn.Micro;");
            sb.AppendLine("using JetBrains.Annotations;");
            sb.AppendLine();
            sb.AppendLine("namespace " + rootNamespace + ".ViewModels");
            sb.AppendLine("{");
            sb.AppendLine("    [UsedImplicitly]");
            sb.AppendLine("    public class ShellViewModel : PropertyChangedBase");
            sb.AppendLine("    {");
            sb.AppendLine();
            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private void AddProjectToSolution(SolutionFolder parent, SolutionItemTemplate project, IList<Project> projects,
            SolutionDataViewModel solutionData)
        {
            if (project is SolutionFolderTemplate)
            {
                AddSolutionFolder(parent, (SolutionFolderTemplate) project, projects, solutionData);
            }
            else
            {
                AddProject(parent, (ProjectTemplate) project, projects);
            }
        }

        private void AddSolutionFolder(SolutionFolder parent, SolutionFolderTemplate solutionFolder,
            IList<Project> projects, SolutionDataViewModel solutionData)
        {
            if (!solutionData.CreateTests &&
                parent == null &&
                solutionFolder.Name == "Tests")
            {
                return;
            }

            if (!solutionData.CreateFakes &&
                solutionFolder.Name == "Fake")
            {
                return;
            }

            var addedProject = parent == null
                ? GetSolution().AddSolutionFolder(solutionFolder.Name)
                : parent.AddSolutionFolder(solutionFolder.Name);

            foreach (var item in solutionFolder.Items)
            {
                AddProjectToSolution(addedProject.Object as SolutionFolder, item, projects, solutionData);
            }
        }

        private void AddProject(SolutionFolder parent, ProjectTemplate project, IList<Project> projects)
        {
            var projectFullName = project.FileName;
            var projectFileName = Path.GetFileName(projectFullName);
            var projectDirectory = Path.GetDirectoryName(projectFullName);

            Debug.Assert(projectDirectory != null, "projectDirectory != null");

            var directoryName = Path.GetDirectoryName(projectDirectory);
            var path1 = Path.GetDirectoryName(directoryName);

            Debug.Assert(path1 != null, "path1 != null");

            var newProjectDirectory = Path.Combine(path1, Path.GetFileName(projectDirectory));

            Debug.Assert(projectFileName != null, "projectFileName != null");

            var newProjectFullName = Path.Combine(newProjectDirectory, projectFileName);

            CopyDirectory(projectDirectory, newProjectDirectory);

            var addedProject = parent == null
                ? GetSolution().AddFromFile(newProjectFullName)
                : parent.AddFromFile(newProjectFullName);

            projects.Add(addedProject);
        }

        /// <summary>
        /// Copies the directory.
        /// </summary>
        /// <param name="sourceDirectory">The source directory.</param>
        /// <param name="destDirectory">The destination directory.</param>
        private static void CopyDirectory(string sourceDirectory, string destDirectory)
        {
            if (!Directory.Exists(destDirectory))
            {
                Directory.CreateDirectory(destDirectory);
            }

            var files = Directory.GetFiles(sourceDirectory);
            foreach (var file in files)
            {
                var name = Path.GetFileName(file);
                if (name == null) continue;

                var dest = Path.Combine(destDirectory, name);
                File.Copy(file, dest);
            }

            var folders = Directory.GetDirectories(sourceDirectory);
            foreach (var folder in folders)
            {
                var name = Path.GetFileName(folder);
                if (name == null) continue;

                var dest = Path.Combine(destDirectory, name);
                CopyDirectory(folder, dest);
            }
        }

        private void CreateSolution(SolutionVariantData solutionVariantData, SolutionDataViewModel solutionData)
        {
            var solution = GetSolution();

            var waitViewModel = new WaitViewModel();
            var window = WpfServices.CreateWindow<WaitView>(waitViewModel);
            WpfServices.SetWindowOwner(window, solution.DTE.MainWindow);
            waitViewModel.Completed += async (s, a) =>
            {
                await window.Dispatcher.InvokeAsync(() =>
                {
                    window.DialogResult = !a.Cancelled && a.Error == null;
                });
            };
            waitViewModel.Caption = "Creating solution...";
            waitViewModel.Run((p, ct) =>
            {
                int count = solutionVariantData.Items.Length;
                double k = 1.0/count;
                for (int i = 0; i < count; ++i)
                {
                    var pr = k*i;
                    p.Report(pr);
                    int j = i;
                    CreateSolutionItem(
                        null,
                        solutionVariantData.Items[i], progress =>
                        {
                            pr = k*j + progress*k;
                            p.Report(pr);
                        },
                        solutionData,
                        ct);
                    ct.ThrowIfCancellationRequested();
                }
            });
            var retVal = window.ShowDialog() ?? false;

            if (!retVal)
            {
                throw new WizardCancelledException();
            }
        }


        private void CreateSolutionItem(
            SolutionFolder solutionFolder,
            SolutionItemData solutionItemData,
            Action<double> progressAction,
            SolutionDataViewModel solutionData,
            CancellationToken ct)
        {
            var solutionFolderData = solutionItemData as SolutionFolderData;
            if (solutionFolderData != null)
            {
                CreateSolutionFolder(
                    solutionFolder,
                    solutionFolderData,
                    progressAction,
                    solutionData,
                    ct);
                return;
            }

            var projectData = solutionItemData as ProjectData;
            if (projectData != null)
            {
                CreateProject(
                    solutionFolder,
                    projectData,
                    progressAction,
                    ct);
                return;
            }

            throw new ArgumentException("Unknown solution item type");
        }

        private void CreateSolutionFolder(
            SolutionFolder solutionFolder,
            SolutionFolderData solutionFolderData,
            Action<double> progressAction,
            SolutionDataViewModel solutionData,
            CancellationToken ct)
        {
            if (!solutionData.CreateTests &&
                solutionFolder == null &&
                solutionFolderData.Name == "Tests")
            {
                return;
            }

            if (!solutionData.CreateFakes &&
                solutionFolderData.Name == "Fake")
            {
                return;
            }

            var addedProject = solutionFolder == null
                ? GetSolution().AddSolutionFolder(solutionFolderData.Name)
                : solutionFolder.AddSolutionFolder(solutionFolderData.Name);

            if (solutionFolderData.Items.Length == 0)
            {
                return;
            }

            SolutionFolder subFolder = addedProject.Object as SolutionFolder;
            var k = 1.0/solutionFolderData.Items.Length;
            for (int i = 0; i < solutionFolderData.Items.Length; ++i)
            {
                var pr = k*i;
                progressAction(pr);
                int j = i;
                CreateSolutionItem(
                    subFolder,
                    solutionFolderData.Items[i], progress =>
                    {
                        pr = k*j + progress*k;
                        progressAction(pr);
                    },
                    solutionData,
                    ct);
                ct.ThrowIfCancellationRequested();
            }
        }

        private void CreateProject(
            SolutionFolder solutionFolder,
            ProjectData projectData,
            Action<double> progressAction,
            CancellationToken ct)
        {
            var projectName = $"{_replacementsDictionary["$safeprojectname$"]}.{projectData.Name}";

            var sourceFileName = Path.Combine(_tmpFolder, projectData.FileName);
            var sourceDir = Path.GetDirectoryName(sourceFileName);
            var solutionDir = _replacementsDictionary["$solutiondirectory$"];
            var destDir = Path.Combine(solutionDir, projectName);

            var replacementDictionary = CreateProjectReplacementDictionary(projectName);
            CopyDirectory(replacementDictionary, sourceDir, destDir, progressAction, ct);

            var oldFileName = Path.GetFileName(sourceFileName);
            var ext = Path.GetExtension(oldFileName);
            var newFileName = projectName + ext;
            var newFullFileName = Path.Combine(destDir, newFileName);

            File.Move(Path.Combine(destDir, oldFileName), newFullFileName);

            var solutionData = _wizardDataViewModel.SelectedSolution;
            ModifyProject(newFullFileName, solutionData);

            var addedProject = solutionFolder == null
                ? GetSolution().AddFromFile(newFullFileName)
                : solutionFolder.AddFromFile(newFullFileName);

            foreach (SolutionConfiguration solutionConfiguration in GetSolution().SolutionBuild.SolutionConfigurations)
            {
                var solutionContext = solutionConfiguration.SolutionContexts
                    .OfType<SolutionContext>()
                    .Single(x => x.ProjectName.EndsWith(newFileName));

                Debug.WriteLine("Project Name: " + projectName);

                var name = solutionConfiguration.Name;
                name = $"{name}|{solutionContext.PlatformName}";

                var projectConfiguration = projectData.ProjectConfigurations.SingleOrDefault(x => x.Name == name);

                if (projectConfiguration == null && System.Diagnostics.Debugger.IsAttached)
                {
                    System.Diagnostics.Debugger.Break();
                }

                Debug.Assert(projectConfiguration != null, "ProjectConfiguration not found for " + name);

                try
                {
                    solutionContext.ConfigurationName = projectConfiguration.ConfigurationName;
                    solutionContext.ShouldBuild = projectConfiguration.IncludeInBuild;
                }
                catch (COMException)
                {
                }
            }

            if (projectData.IsStartup)
            {
                GetSolution().Properties.Item("StartupProject").Value = addedProject.Name;
            }

            progressAction(1.0);
        }

        private Dictionary<string, string> CreateProjectReplacementDictionary(string projectName)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (var key in _replacementsDictionary.Keys)
            {
                result[key] = _replacementsDictionary[key];
            }

            result["$safeprojectname$"] = projectName;
            result["$projectname$"] = projectName;

            return result;
        }

        private void CopyDirectory(
            Dictionary<string, string> replacementsDictionary,
            string sourceDir,
            string destDir,
            Action<double> progressAction,
            CancellationToken ct)
        {
            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }

            var infos = new DirectoryInfo(sourceDir).GetFileSystemInfos();
            if (infos.Length == 0)
            {
                return;
            }

            var k = 0.8 / infos.Length;

            for (int i = 0; i < infos.Length; ++i)
            {
                ct.ThrowIfCancellationRequested();

                var pr = k * i;
                progressAction(pr);
                int j = i;

                var info = infos[i];

                var destFileName = Path.Combine(destDir, info.Name);
                if (info is DirectoryInfo)
                {
                    CopyDirectory(
                        replacementsDictionary,
                        info.FullName,
                        destFileName,
                        progress =>
                        {
                            pr = k * j + progress * k;
                            progressAction(pr);
                        },
                        ct);
                    continue;
                }

                CopyProject(replacementsDictionary, info.FullName, destFileName);
            }
        }

        private void CopyProject(
            Dictionary<string, string> replacementsDictionary,
            string oldFileName,
            string newFileName)
        {
            var ext = Path.GetExtension(newFileName).ToLower();

            switch (ext)
            {
                case ".cs":
                case ".xaml":
                case ".config":
                case ".csproj":
                    ReplaceText(replacementsDictionary, oldFileName, newFileName);
                    break;
                default:
                    File.Copy(oldFileName, newFileName);
                    break;
            }
        }

        private void ReplaceText(
            Dictionary<string, string> replacementsDictionary,
            string oldFileName,
            string newFileName)
        {
            var str = File.ReadAllText(oldFileName);
            foreach (var sp in replacementsDictionary)
            {
                str = str.Replace(sp.Key, sp.Value);
            }
            File.WriteAllText(newFileName, str);
        }

        #endregion

        #region Overrides

        protected override void RunStartedOverride(Solution4 solution, Dictionary<string, string> replacementsDictionary, object[] customParams)
        {
            _replacementsDictionary = replacementsDictionary;

            var vstemplateName = (string)customParams[0];
            _tmpFolder = Path.GetDirectoryName(vstemplateName);
            replacementsDictionary["$saferootprojectname$"] = replacementsDictionary["$safeprojectname$"];

            var wizardDataFileName = Path.Combine(_tmpFolder, WizardDataLoader.WizardDataFielName);
            var wizardData = WizardDataLoader.LoadAsync(wizardDataFileName).Result;

            _wizardDataViewModel = null;

            if (wizardData == null)
            {
                return;
            }

            var projectName = replacementsDictionary["$projectname$"];

            _wizardDataViewModel = new WizardDataViewModel(wizardData)
            {
                Title = $"{wizardData.Title} - {projectName}"
            };

            var solutions = _wizardDataViewModel.Solutions.ToArray();

            if (solutions.Length == 1)
            {
                var solutionDataViewModel = solutions[0];
                if (solutionDataViewModel.UseOnlyDefautValues)
                {
                    solutionDataViewModel.SelectedVariant = solutionDataViewModel.Variants.First();
                    _wizardDataViewModel.SetSelectedSolution(solutionDataViewModel);
                    return;
                }
            }

            var window = WpfServices.CreateWindow<WizardWindow>(_wizardDataViewModel);
            WpfServices.SetWindowOwner(window, solution.DTE.MainWindow);
            var retVal = window.ShowDialog() ?? false;
            if (!retVal)
            {
                throw new WizardCancelledException();
            }
        }

        protected override void RunFinishedOverride()
        {
            var solutionData = _wizardDataViewModel.SelectedSolution;
            var solutionVariant = _wizardDataViewModel.SelectedSolution.SelectedVariant;

            CreateSolution(solutionVariant.Model, solutionData);

            if (!string.IsNullOrWhiteSpace(solutionData.Model.PostCreateUrl))
            {
                GetSolution().DTE.ItemOperations.Navigate(solutionData.Model.PostCreateUrl);
            }

            //Get all projects in solution
            var projects = GetProjects().ToArray();
            if (projects == null || !projects.Any())
            {
                throw new Exception("No projects found.");
            }
        }

        #endregion
    }
}