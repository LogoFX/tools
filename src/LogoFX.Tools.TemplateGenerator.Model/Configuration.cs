using System.Collections.Generic;
using System.Collections.ObjectModel;
using LogoFX.Tools.TemplateGenerator.Model.Contract;

namespace LogoFX.Tools.TemplateGenerator.Model
{
    internal sealed class Configuration : IConfiguration
    {
        public Configuration()
        {
            Solutions = new ObservableCollection<SolutionConfiguration>();
        }

        public IList<SolutionConfiguration> Solutions { get; private set; }

        IEnumerable<ISolutionConfiguration> IConfiguration.Solutions
        {
            get { return Solutions; }
        }
    }
}