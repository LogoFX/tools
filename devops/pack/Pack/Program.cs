using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Pack
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            GoUp(5);
            var single = args != null && args.Length > 0 ? args[0] : null;           
            if (single != null)
            {
                PackSingle(Path.Combine(Directory.GetCurrentDirectory(), single));
                return;
            }
            
            foreach (var dir in Directory.GetDirectories(Directory.GetCurrentDirectory()))
            {
                if (dir.EndsWith("tools")) continue;
                PackSingle(dir);                
            }
        }

        private static void PackSingle(string dir)
        {
             Console.WriteLine(dir);             
                var devopsDir = Path.Combine(dir, "devops");
                if (Directory.Exists(devopsDir))
                {
                    Directory.SetCurrentDirectory(devopsDir);
                    var packDir = Path.Combine(devopsDir, "pack");
                    if (Directory.Exists(packDir))
                    {
                        Directory.SetCurrentDirectory(packDir);
                        var packBat = Path.Combine(packDir, "pack.bat");
                        if (File.Exists(packBat))
                        {
                            var process = Process.Start(packBat);
                            process.WaitForExit();
                        }
                        else
                        {
                            var packAllBat = Path.Combine(packDir, "pack-all.bat");
                            if (File.Exists(packAllBat))
                            {
                                var process = Process.Start(packAllBat);
                                process.WaitForExit();
                            }
                        }
                        GoUp(3);
                    }
                    else
                    {
                       GoUp(2); 
                    }
                }
        }

        private static void GoUp(int numberOfLevels)
        {
            var relativePath = new List<string> { Directory.GetCurrentDirectory() };
            relativePath.AddRange(Enumerable.Repeat("..", numberOfLevels));
            Directory.SetCurrentDirectory(Path.Combine(relativePath.ToArray()));
        }
    }
}
