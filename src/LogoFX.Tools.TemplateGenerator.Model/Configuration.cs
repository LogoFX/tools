using System.Collections.Generic;
using System.Collections.ObjectModel;
using JetBrains.Annotations;
using LogoFX.Tools.TemplateGenerator.Model.Contract;

namespace LogoFX.Tools.TemplateGenerator.Model
{
    [UsedImplicitly]
    internal sealed class Configuration : IConfiguration
    {
        public ObservableCollection<SolutionConfiguration> Solutions { get; [UsedImplicitly] private set; }

        IEnumerable<ISolutionConfiguration> IConfiguration.Solutions
        {
            get { return Solutions; }
        }
    }
}