using System;
using System.Collections.Generic;
using Solid.Core;

namespace PublishUtil
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var single = args != null && args.Length > 0 ? args[0] : null;
            var directories = InitTopology();
        }

        private static IEnumerable<IPackageGroup> InitTopology()
        {
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
            directories = directories.SortTopologically();
            return directories;
        }
    }

    internal interface IPackageGroup : IHaveDependencies, IIdentifiable
    {

    }

    internal sealed class PackageGroup : IPackageGroup
    {
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
}
