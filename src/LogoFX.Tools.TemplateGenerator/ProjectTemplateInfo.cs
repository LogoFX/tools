using System;
using System.Collections.Generic;
using LogoFX.Tools.TemplateGenerator.Contracts;

namespace LogoFX.Tools.TemplateGenerator
{
    internal sealed class ProjectTemplateInfo : SolutionItemTemplateInfo, IProjectTemplateInfo
    {
        public ProjectTemplateInfo(Guid id, string name) 
            : base(id, name)
        {
            ProjectConfigurations = new ProjectConfiguration[0];
        }

        string IProjectTemplateInfo.NameWithoutRoot => NameWithoutRoot;

        public string NameWithoutRoot { get; set; }

        string IProjectTemplateInfo.FileName => FileName;

        public string FileName { get; set; }

        string IProjectTemplateInfo.DestinationFileName => DestinationFileName;
        public string DestinationFileName { get; private set; }

        public void SetDestinationFileName(string destinationFileName)
        {
            DestinationFileName = destinationFileName;
        }

        IEnumerable<IProjectConfiguration> IProjectTemplateInfo.ProjectConfigurations
        {
            get { return ProjectConfigurations; }
        }

        public ProjectConfiguration[] ProjectConfigurations { get; set; }
    }
}