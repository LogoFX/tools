using System;
using System.Collections.Generic;
using LogoFX.Tools.TemplateGenerator.Contracts;

namespace LogoFX.Tools.TemplateGenerator
{
    internal class SolutionFolderTemplateInfo : SolutionItemTemplateInfo, ISolutionFolderTemplateInfo
    {
        private readonly bool _createOnDisk;

        private readonly List<SolutionItemTemplateInfo> _items =
            new List<SolutionItemTemplateInfo>();

        public SolutionFolderTemplateInfo(Guid id, string name, bool createOnDisk)
            : base(id, name)
        {
            _createOnDisk = createOnDisk;
        }

        IEnumerable<ISolutionItemTemplateInfo> ISolutionFolderTemplateInfo.Items => Items;

        public IList<SolutionItemTemplateInfo> Items => _items;

        public bool CreateOnDisk => _createOnDisk;
    }
}