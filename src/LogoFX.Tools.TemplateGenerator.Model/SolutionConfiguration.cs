using LogoFX.Tools.TemplateGenerator.Model.Contract;

namespace LogoFX.Tools.TemplateGenerator.Model
{
    internal sealed class SolutionConfiguration : AppModel, ISolutionConfiguration
    {
        private string _engineName;

        public string EngineName
        {
            get => _engineName;
            set => Set(ref _engineName, value);
        }

        private string _path;

        public string Path
        {
            get => _path;
            set => Set(ref _path, value);
        }

        private string _iconPath;

        public string IconPath
        {
            get => _iconPath;
            set => Set(ref _iconPath, value);
        }

        private string _postCreateUrl;

        public string PostCreateUrl
        {
            get => _postCreateUrl;
            set => Set(ref _postCreateUrl, value);
        }

        private string _defaultName;

        public string DefaultName
        {
            get => _defaultName;
            set => Set(ref _defaultName, value);
        }

        private string _templateFolder;

        public string TemplateFolder
        {
            get => _templateFolder;
            set => Set(ref _templateFolder, value);
        }
    }
}