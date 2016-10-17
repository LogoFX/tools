using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LogoFX.Tools.TemplateGenerator.Contracts;

namespace LogoFX.Tools.TemplateGenerator
{
    internal sealed class SolutionTemplateInfo : SolutionFolderTemplateInfo, ISolutionTemplateInfo
    {
        private readonly ObservableCollection<string> _rootNamespaces =
            new ObservableCollection<string>();

        public SolutionTemplateInfo(string containerName)
            : base(Guid.Empty, string.Empty)
        {
            ContainerName = containerName;
        }

        public string ContainerName { get; private set; }

        public ICollection<string> RootNamespaces => _rootNamespaces;
    }
}