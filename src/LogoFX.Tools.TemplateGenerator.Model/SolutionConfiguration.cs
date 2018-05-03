using LogoFX.Tools.TemplateGenerator.Model.Contract;

namespace LogoFX.Tools.TemplateGenerator.Model
{
    internal sealed class SolutionConfiguration : AppModel, ISolutionConfiguration
    {
        private string _path;

        private ISolutionConfigurationPlugin _plugin;

        public ISolutionConfigurationPlugin Plugin
        {
            get { return _plugin; }
            set { Set(ref _plugin, value); }
        }

        public string Path
        {
            get { return _path; }
            set { Set(ref _path, value); }
        }

        private string _startupProjectName;

        public string StartupProjectName
        {
            get { return _startupProjectName; }
            set { Set(ref _startupProjectName, value); }
        }
    }
}