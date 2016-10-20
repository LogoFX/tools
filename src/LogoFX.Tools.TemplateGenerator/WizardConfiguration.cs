using System.Collections.ObjectModel;

namespace LogoFX.Tools.TemplateGenerator
{
    public sealed class WizardConfiguration
    {
        public WizardConfiguration()
        {
            ProjectType = "CSharp";
            SortOrder = 5000;
            Solutions = new ObservableCollection<SolutionInfo>();
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public string DefaultName { get; set; }

        public string IconName { get; set; }

        public ObservableCollection<SolutionInfo> Solutions { get; private set; }

        public string ProjectType { get; set; }

        public int SortOrder { get; set; }
    }
}