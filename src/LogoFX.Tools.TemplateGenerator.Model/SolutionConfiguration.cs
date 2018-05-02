using LogoFX.Tools.TemplateGenerator.Model.Contract;

namespace LogoFX.Tools.TemplateGenerator.Model
{
    internal sealed class SolutionConfiguration : AppModel, ISolutionConfiguration
    {
        private string _path;

        public string Path
        {
            get { return _path; }
            set { Set(ref _path, value); }
        }

        private bool _vs2017;

        public bool Vs2017
        {
            get { return _vs2017; }
            set { Set(ref _vs2017, value); }
        }

        private string _startupProjectName;

        public string StartupProjectName
        {
            get { return _startupProjectName; }
            set { Set(ref _startupProjectName, value); }
        }
    }
}