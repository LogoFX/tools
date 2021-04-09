using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Common;

namespace PublishUtil
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var single = args != null && args.Length > 0 ? args[0] : null;
            var packageGroups = TopologyUtils.InitTopology();
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
    }
}
