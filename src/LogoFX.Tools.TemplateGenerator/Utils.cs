﻿using System;
using System.Diagnostics;
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

        public static string GetRelativePath(string filespec, string folder)
        {
            if (filespec == null)
            {
                Debugger.Break();
            }

            Debug.Assert(filespec != null, nameof(filespec) + " != null");
            Uri pathUri = new Uri(filespec);
            // Folders must end in a slash
            if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                folder += Path.DirectorySeparatorChar;
            }
            Uri folderUri = new Uri(folder);
            return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
        }
    }
}