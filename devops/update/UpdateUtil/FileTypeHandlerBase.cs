﻿using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using YamlDotNet.RepresentationModel;

namespace UpdateUtil
{
    abstract class FileTypeHandlerBase
    {
        protected string SubDirectory { get; }

        protected FileTypeHandlerBase(string subDirectory)
        {
            SubDirectory = subDirectory;
        }

        public virtual void SetCurrentDirectoryBefore()
        {
            NavigationHelper.GoUp(5);
            NavigationHelper.Cd(SubDirectory);
        }

        public virtual void SetCurrentDirectoryAfter()
        {
            NavigationHelper.GoUp(1);
            NavigationHelper.Cd("tools");
            NavigationHelper.Cd("devops");
            NavigationHelper.NavigateToBin();
        }

        public abstract void UpdateFiles(string prefix, VersionInfo versionInfo);
    }

    class SdkProjectFileTypeHandler : FileTypeHandlerBase
    {
        public SdkProjectFileTypeHandler(string subDirectory)
        :base(subDirectory)
        {
            
        }

        public override void UpdateFiles(string prefix, VersionInfo versionInfo)
        {
            var version = versionInfo.VersionCore;
            var projectFiles =
                Directory.GetFiles(Directory.GetCurrentDirectory(), "*.csproj", SearchOption.AllDirectories);
         
            foreach (var projectFile in projectFiles)
            {
                if (projectFile.Contains("templatepack"))
                {
                    continue;
                }
                var doc = new XmlDocument();
                doc.Load(projectFile);
                var firstPropertyGroup = doc.GetElementsByTagName("Project")[0]["PropertyGroup"];
                var targetFrameworkElement = firstPropertyGroup?.GetElementsByTagName("TargetFramework")[0];
                if (targetFrameworkElement == null)
                {
                    continue;
                }

                if (!targetFrameworkElement.InnerText.StartsWith("netcoreapp") &&
                    !targetFrameworkElement.InnerText.StartsWith("net5") &&
                    !targetFrameworkElement.InnerText.StartsWith("netstandard"))
                {
                    continue;
                }
                var versionElement = doc.GetElementsByTagName("Project")[0]["PropertyGroup"].GetElementsByTagName("Version")[0];
                if (versionElement != null)
                {
                    versionElement.InnerText = version;
                }
                doc.Save(projectFile);
            }
        }
    }

    class AssemblyInfoFileTypeHandler : FileTypeHandlerBase
    {
        public AssemblyInfoFileTypeHandler(string subDirectory)
            : base(subDirectory)
        {

        }

        public override void UpdateFiles(string prefix, VersionInfo versionInfo)
        {
            var assemblyInfoFiles =
                Directory.GetFiles(Directory.GetCurrentDirectory(), "AssemblyInfo.cs", SearchOption.AllDirectories)
                    .Where(t => !t.Contains("obj"));
            var ps1File = @"../tools/devops/update/patch-assembly-info.ps1";

            foreach (var assemblyInfoFile in assemblyInfoFiles)
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = $"-NoProfile -ExecutionPolicy unrestricted -File \"{ps1File}\" \"{assemblyInfoFile}\" \"{versionInfo.VersionCore}\"",
                    UseShellExecute = false
                };
                var process = Process.Start(startInfo);
                process.WaitForExit();
            }
        }
    }

    class CIFileTypeHandler : FileTypeHandlerBase
    {
        public CIFileTypeHandler(string subDirectory)
            : base(subDirectory)
        {

        }

        public override void UpdateFiles(string prefix, VersionInfo versionInfo)
        {
            var ciFile = "appveyor.yml";
            var yaml = new YamlStream();
            using (var reader = new StreamReader(ciFile))
            {
                // Load the stream
                yaml.Load(reader);
                var nodesEnumerator = yaml.Documents[0].RootNode.AllNodes.GetEnumerator();
                nodesEnumerator.MoveNext();
                var rootNode = nodesEnumerator.Current as YamlMappingNode;
                var versionNode = rootNode.Children[new YamlScalarNode("version")] as YamlScalarNode;
                versionNode.Value = $"{versionInfo.VersionCore}.{{build}}";
            }

            using (var writer = new StreamWriter(ciFile))
            {
                yaml.Save(writer, assignAnchors: false);
            }
        }
    }

    class ManifestFileTypeHandler : FileTypeHandlerBase
    {
        private readonly ManifestFileTypeHandlerOptions _options;

        public ManifestFileTypeHandler(string subDirectory, ManifestFileTypeHandlerOptions options)
            :base(subDirectory)
        {
            _options = options;
        }

        public ManifestFileTypeHandler(string subDirectory)
            :this(subDirectory, new ManifestFileTypeHandlerOptions())
        {

        }

        public override void SetCurrentDirectoryBefore()
        {
            NavigationHelper.GoUp(5);
            NavigationHelper.Cd(SubDirectory);
            NavigationHelper.Cd("devops");
            NavigationHelper.Cd("pack");
        }

        public override void SetCurrentDirectoryAfter()
        {
            NavigationHelper.GoUp(3);
            NavigationHelper.Cd("tools");
            NavigationHelper.Cd("devops");
            NavigationHelper.NavigateToBin();
        }

        public override void UpdateFiles(string prefix, VersionInfo versionInfo)
        {
            var version = versionInfo.ToString();
            var manifestFiles =
                Directory.GetFiles(Directory.GetCurrentDirectory(), $"*.nuspec", SearchOption.AllDirectories);
            foreach (var manifestFile in manifestFiles)
            {
                var doc = new XmlDocument();
                doc.Load(manifestFile);
                if (_options.UpdatePackageVersion)
                {
                    var versionElement = doc.GetElementsByTagName("package")[0]["metadata"]["version"];
                    versionElement.InnerText = version;
                }
                if (_options.UpdateDependencyVersion)
                {
                    var dependenciesElement = doc.GetElementsByTagName("package")[0]["metadata"]["dependencies"];
                    if (dependenciesElement != null)
                    {
                        var dependencies = dependenciesElement.GetElementsByTagName("dependency");
                        foreach (XmlNode dependencyElement in dependencies)
                        {
                            if (dependencyElement.Attributes["id"].Value.StartsWith(prefix))
                            {
                                dependencyElement.Attributes["version"].Value = version;
                            }
                        }
                    }
                }
                
                doc.Save(manifestFile);
            }
        }
    }

    class ManifestFileTypeHandlerOptions
    {
        public ManifestFileTypeHandlerOptions()
        {
            
        }

        public bool UpdatePackageVersion {get;set;}
        public bool UpdateDependencyVersion {get;set;}
    }
}