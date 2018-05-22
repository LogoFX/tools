using System;
using System.Collections.Generic;

namespace LogoFX.Tools.TemplateGenerator.Engine.Shared
{
    public class SolutionFolderTemplateInfo : SolutionItemTemplateInfo
    {

        private readonly List<SolutionItemTemplateInfo> _items =
            new List<SolutionItemTemplateInfo>();

        public SolutionFolderTemplateInfo(Guid id, string name)
            : base(id, name)
        {
        }

        public IList<SolutionItemTemplateInfo> Items => _items;
    }
}