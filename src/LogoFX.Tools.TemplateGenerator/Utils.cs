using System;
using System.IO;

namespace LogoFX.Tools.TemplateGenerator
{
    public static class Utils
    {
        public static bool FileNamesAreEqual(string fileName1, string fileName2)
        {
            return string.Compare(fileName1, fileName2, StringComparison.OrdinalIgnoreCase) == 0;
        }

        public static string SolutionFileNameToName(string fileName)
        {
            return Path.GetFileNameWithoutExtension(fileName);
        }
    }
}