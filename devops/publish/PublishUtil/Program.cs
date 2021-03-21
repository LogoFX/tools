using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Solid.Core;

namespace PublishUtil
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var single = args != null && args.Length > 0 ? args[0] : null;
            var packageGroups = InitTopology();
            GoUp(5);
            foreach (var packageGroup in packageGroups)
            {
                PublishGroup(packageGroup.Id);
            }
        }

        private static void PublishGroup(string packageGroupId)
        {
            Directory.SetCurrentDirectory(Path.Combine(Directory.GetCurrentDirectory(), packageGroupId));
            var rootDir = Directory.GetCurrentDirectory();
            if (Directory.Exists(Path.Combine(rootDir, "devops")))
            {
                Directory.SetCurrentDirectory(Path.Combine(rootDir, "devops"));
                if (Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "publish")))
                {
                    Directory.SetCurrentDirectory(Path.Combine(Directory.GetCurrentDirectory(), "publish"));
                    var publishBat = Path.Combine(Directory.GetCurrentDirectory(), "publish.bat");
                    if (File.Exists(publishBat))
                    {
                        StartAndWait(publishBat);
                    }
                    else
                    {
                        publishBat = Path.Combine(Directory.GetCurrentDirectory(), "publish-all.bat");
                        if (File.Exists(publishBat))
                        {
                            StartAndWait(publishBat);
                        }
                    }

                    GoUp(3);
                    return;
                }

                GoUp(2);
                return;
            }
            GoUp(1);
        }

        private static void GoUp(int numberOfLevels)
        {
            var relativePath = new List<string> { Directory.GetCurrentDirectory() };
            relativePath.AddRange(Enumerable.Repeat("..", numberOfLevels));
            Directory.SetCurrentDirectory(Path.Combine(relativePath.ToArray()));
        }

        private static void StartAndWait(string path)
        {
            var args = new[] {"../../../../packages/Tests-All"};
            var process = Process.Start(path, args);
            process.WaitForExit();
        }

        private static IEnumerable<IPackageGroup> InitTopology()
        {
            var contents = File.ReadAllText("topology.json");
            var data = JsonConvert.DeserializeObject<Topology>(contents).Data;
            
            //TODO: Remove as this info is fetched from the topology file
            IEnumerable<IPackageGroup> directories = new[]
            {
                new PackageGroup("cr"),
                new PackageGroup("ioc", new[] {"cr"}),
                new PackageGroup("cl-md"),
                new PackageGroup("cl-cr", new[] {"cr"}),
                new PackageGroup("sr-bs"),
                new PackageGroup("cl-ts-cn"),
                new PackageGroup("cl-ts-sh-caliburn-micro"),
                new PackageGroup("cl-ts-sh", new[] {"cl-cr"}),
                new PackageGroup("cl-ts-in", new[] {"cl-ts-cn"}),
                new PackageGroup("cl-ts-e2e", new[] {"cl-ts-cn"}),
                new PackageGroup("cl-ts-in-nunit", new[] {"cl-ts-sh"}),
                new PackageGroup("cl-ts-in-xunit", new[] {"cl-ts-sh"}),
                new PackageGroup("cl-ts-in-sf", new[] {"cl-ts-sh"}),
                new PackageGroup("cl-ts-e2e-sf"),
                new PackageGroup("cl-ts-e2e-white"),
                new PackageGroup("cl-ts-e2e-flaui"),
                new PackageGroup("cl-bs-ad-cn"),
                new PackageGroup("cl-bs-ad-unity", new[] {"cl-bs-ad-cn"}),
                new PackageGroup("cl-bs-ad-simple-container", new[] {"cl-bs-ad-cn"}),
                new PackageGroup("cl-mvvm-cr", new[] {"cr"}),
                new PackageGroup("cl-mvvm-cm", new[] {"cl-cr", "cl-mvvm-cr"}),
                new PackageGroup("cl-mvvm-m", new[] {"cr", "cl-cr"}),
                new PackageGroup("cl-mvvm-vm", new[] {"cr", "cl-cr", "cl-mvvm-cr"}),
                new PackageGroup("cl-mvvm-vmf", new[] {"cl-mvvm-vm"}),
                new PackageGroup("cl-mvvm-vmf-simple-cnt", new[] {"ioc", "cl-mvvm-vm"}),
                new PackageGroup("cl-mvvm-vmf-unity", new[] {"cl-mvvm-vm"}),
                new PackageGroup("cl-mvvm-vm-ex",
                    new[]
                    {
                        "cr", "cl-mvvm-cm", "cl-mvvm-cr", "cl-mvvm-m",
                        "cl-mvvm-vm", "cl-mvvm-vmf", "cl-bs",
                        "cl-bs-ad-simple-container", "cl-ts-in-xunit", "cl-ts-sh-caliburn-micro"
                    }),
                new PackageGroup("cl-mvvm-v", new[] {"cl-cr"}),
                new PackageGroup("cl-mvvm-v-ex", new[] {"cl-mvvm-cr", "cl-mvvm-vm-ex"}),
                new PackageGroup("bs"),
                new PackageGroup("cl-bs",
                    new[]
                    {
                        "bs", "cl-bs-ad-cn", "cr", "cl-cr",
                        "cl-bs-ad-simple-container", "cl-ts-sh-caliburn-micro"
                    }),
            };
            data = data.SortTopologically().ToList();
            return data;
        }
    }

    internal interface IPackageGroup : IHaveDependencies, IIdentifiable
    {

    }

    internal sealed class PackageGroup : IPackageGroup
    {
        [JsonConstructor]
        public PackageGroup(
            string id, 
            string[] dependencies)
        {
            Dependencies = dependencies;
            Id = id;
        }

        public PackageGroup(
            string id)
        {
            Dependencies = Array.Empty<string>();
            Id = id;
        }

        public override string ToString()
        {
            return Id;
        }

        public string[] Dependencies { get; }
        public string Id { get; }
    }

    internal sealed class Topology
    {
        public List<PackageGroup> Data { get; set; }
    }
}
