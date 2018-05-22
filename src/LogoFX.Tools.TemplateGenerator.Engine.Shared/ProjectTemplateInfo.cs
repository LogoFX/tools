using System;

namespace LogoFX.Tools.TemplateGenerator.Engine.Shared
{
    public sealed class ProjectTemplateInfo : SolutionItemTemplateInfo
    {
        public ProjectTemplateInfo(Guid id, string name) 
            : base(id, name)
        {
        }

        public string NameWithoutRoot { get; set; }

        public string FileName { get; set; }

        public string DestinationFileName { get; set; }

        public bool IsStartup { get; set; }
    }
}