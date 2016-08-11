using System;

namespace LogoFX.Tools.TemplateGenerator
{
    internal sealed class ProjectTemplateInfo : SolutionItemTemplateInfo, IProjectTemplateInfo
    {
        public ProjectTemplateInfo(Guid id, string name) 
            : base(id, name)
        {
        }

        string IProjectTemplateInfo.NameWithoutRoot => NameWithoutRoot;

        public string NameWithoutRoot { get; set; }

        string IProjectTemplateInfo.FileName => FileName;

        public string FileName { get; set; }
    }
}