using System.Xml.Linq;
using LogoFX.Tools.TemplateGenerator.Data.Contracts.Dto;

namespace LogoFX.Tools.TemplateGenerator.Engine.SamplesSpecification
{
    internal sealed partial class TemplateGeneratorEngine
    {
        private static readonly XNamespace _ns = XNamespace.Get("http://schemas.microsoft.com/developer/vstemplate/2005");

        private XDocument CreateDefinitionInternal(SolutionConfigurationDto solutionConfiguration)
        {
            var projectCollection = new XElement(_ns + "ProjectCollection");

            var doc = new XDocument(
                new XElement(_ns + "VSTemplate",
                    new XAttribute("Version", "3.0.0"),
                    new XAttribute("Type", "ProjectGroup"),
                    new XElement(_ns + "TemplateData",
                        new XElement(_ns + "Name", solutionConfiguration.Name),
                        new XElement(_ns + "Description", solutionConfiguration.Description),
                        new XElement(_ns + "ProjectType", "CSharp"),
                        new XElement(_ns + "DefaultName", solutionConfiguration.DefaultName),
                        new XElement(_ns + "SortOrder", 5000),
                        new XElement(_ns + "Icon", solutionConfiguration.IconPath)),
                    new XElement(_ns + "TemplateContent", projectCollection),
                    MakeWizardExtension(
                        "LogoFX.Tools.Templates.Wizard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
                        $"LogoFX.Tools.Templates.Wizard.SolutionWizard")
                ));

            return doc;
        }

        private XElement MakeWizardExtension(string assemblyName, string className)
        {
            return new XElement(_ns + "WizardExtension",
                new XElement(_ns + "Assembly", assemblyName),
                new XElement(_ns + "FullClassName", className));
        }

        public XDocument CreatePreprocessDocument(SolutionConfigurationDto solutionConfiguration)
        {
            var doc = new XDocument(
                new XElement("Preprocess",
                    new XElement("TemplateInfo",
                        new XAttribute("Path", "CSharp\\LogoFX")),
                    new XElement("Replacements",
                        new XAttribute("Include", "*.*"),
                        new XAttribute("Exclude", "*.vstemplate;*.csproj;*.fsproj;*.vbproj;*.jpg;*.png;*.ico;_preprocess.xml;_project.vstemplate.xml"))));

            return doc;
        }
    }
}