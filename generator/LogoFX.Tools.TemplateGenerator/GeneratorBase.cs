using System;
using System.IO;
using System.Xml.Linq;

namespace LogoFX.Tools.TemplateGenerator
{
    public abstract class GeneratorBase
    {
        private static readonly string[] s_names = { ".Client.", ".Tests." };

        protected static readonly XNamespace s_ns = XNamespace.Get("http://schemas.microsoft.com/developer/vstemplate/2005");

        protected string GetRootName(string projectName)
        {
            projectName = Path.GetFileName(projectName);

            int index = -1;
            foreach (var name in s_names)
            {
                index = projectName.IndexOf(name, StringComparison.InvariantCulture);
                if (index >= 0)
                {
                    break;
                }
            }

            var rootName = projectName.Substring(0, index);

            return rootName;
        }

        protected string SafeProjectName(IProjectTemplateInfo projectTemplateInfo)
        {
            return $"$safeprojectname$.{projectTemplateInfo.NameWithoutRoot}";
        }

        protected string SafeRootProjectName(IProjectTemplateInfo projectTemplateInfo)
        {
            return $"$saferootprojectname$.{projectTemplateInfo.NameWithoutRoot}";
        }
    }
}