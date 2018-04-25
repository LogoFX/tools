using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using LogoFX.Tools.TemplateGenerator.Contracts;

namespace LogoFX.Tools.TemplateGenerator
{
    public abstract class GeneratorBase
    {
        private static readonly string[] _names = { ".Client.", ".Tests." };

        protected static readonly XNamespace Ns = XNamespace.Get("http://schemas.microsoft.com/developer/vstemplate/2005");

        protected string GetRootName(string projectName)
        {
            projectName = Path.GetFileName(projectName);

            Debug.Assert(projectName != null, nameof(projectName) + " != null");

            int index = -1;
            foreach (var name in _names)
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

        protected XElement MakeWizardExtension(string assemblyName, string className)
        {
            return new XElement(Ns + "WizardExtension",
                new XElement(Ns + "Assembly", assemblyName),
                new XElement(Ns + "FullClassName", className));
        }
    }
}