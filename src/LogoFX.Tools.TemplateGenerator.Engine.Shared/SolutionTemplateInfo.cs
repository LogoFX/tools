using System;

namespace LogoFX.Tools.TemplateGenerator.Engine.Shared
{
    public sealed class SolutionTemplateInfo : SolutionFolderTemplateInfo
    {
        public SolutionTemplateInfo(string name)
            : base(Guid.Empty, name)
        {
        }

        public string Description { get; set; }
        public string IconPath { get; set; }
        public string PostCreateUrl { get; set; }
    }
}