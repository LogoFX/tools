using System;
using System.Collections.Generic;

namespace UpdateUtil
{
    static class Program
    {
        static void Main(string[] args)
        {
            var command = args[0];
            var prefix = args[1];
            var version = args.Length >= 3 ? args[2] : string.Empty;
            if (version == string.Empty)
            {
                Console.WriteLine("No version provided");
                return;
            }
            var versionInfo = new VersionInfo(version);

            var packageGroups = Common.TopologyUtils.InitTopology();
            foreach (var packageGroup in packageGroups)
            {
                var subDirectory = packageGroup.Id;
                var handlers = new List<FileTypeHandlerBase>();
                switch (command)
                {
                    case "bump-version":
                        handlers.Add(new SdkProjectFileTypeHandler(subDirectory));
                        handlers.Add(new AssemblyInfoFileTypeHandler(subDirectory));
                        handlers.Add(new CIFileTypeHandler(subDirectory));
                        handlers.Add(new ManifestFileTypeHandler(subDirectory, new ManifestFileTypeHandlerOptions
                        {
                            UpdatePackageVersion = true,
                            UpdateDependencyVersion = true
                        }));
                        break;
                    case "bump-dependency-version":
                        handlers.Add(new ManifestFileTypeHandler(subDirectory, new ManifestFileTypeHandlerOptions
                        {
                            UpdateDependencyVersion = true
                        }));
                        break;
                }

                foreach (var handler in handlers)
                {
                    handler.SetCurrentDirectoryBefore();
                    handler.UpdateFiles(prefix, versionInfo);
                    handler.SetCurrentDirectoryAfter();
                }
            }
        }
    }
}