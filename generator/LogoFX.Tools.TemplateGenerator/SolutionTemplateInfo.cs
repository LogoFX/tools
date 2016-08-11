using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LogoFX.Tools.TemplateGenerator
{
    internal sealed class SolutionTemplateInfo : SolutionFolderTemplateInfo, ISolutionTemplateInfo
    {
        private readonly ObservableCollection<string> _rootNamespaces =
            new ObservableCollection<string>();

        public SolutionTemplateInfo()
            : base(Guid.Empty, string.Empty)
        {
        }

        public ICollection<string> RootNamespaces => _rootNamespaces;
    }
}