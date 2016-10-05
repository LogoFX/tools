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

        public bool TestOption { get; set; }

        public bool FakeOption { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string DefaultName { get; set; }

        public string IconName { get; set; }

        public ObservableCollection<SolutionInfo> Solutions { get; }

        public string ProjectType { get; set; }

        public int SortOrder { get; set; }
    }
}