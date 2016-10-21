using Caliburn.Micro;

namespace LogoFX.Tools.TemplateGenerator
{
    public sealed class SolutionInfo : PropertyChangedBase
    {
        private string _name;
        private string _caption;
        private string _iconName;
        private string _postCreateUrl;
        private SolutionOptionsInfo _options;
        private SolutionVariant[] _solutionVariants;

        public SolutionInfo()
        {
            SolutionVariants = new SolutionVariant[0];
            Options = new SolutionOptionsInfo();
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name)
                {
                    return;
                }
                _name = value;
                NotifyOfPropertyChange();
            }
        }

        public string Caption
        {
            get { return _caption; }
            set
            {
                if (value == _caption)
                {
                    return;
                }
                _caption = value;
                NotifyOfPropertyChange();
            }
        }

        public string IconName
        {
            get { return _iconName; }
            set
            {
                if (value == _iconName)
                {
                    return;
                }
                _iconName = value;
                NotifyOfPropertyChange();
            }
        }

        public string PostCreateUrl
        {
            get { return _postCreateUrl; }
            set
            {
                if (value == _postCreateUrl)
                {
                    return;
                }
                _postCreateUrl = value;
                NotifyOfPropertyChange();
            }
        }

        public SolutionOptionsInfo Options
        {
            get { return _options; }
            set
            {
                if (Equals(value, _options))
                {
                    return;
                }
                _options = value;
                NotifyOfPropertyChange();
            }
        }

        public SolutionVariant[] SolutionVariants
        {
            get { return _solutionVariants; }
            set
            {
                if (Equals(value, _solutionVariants))
                {
                    return;
                }
                _solutionVariants = value;
                NotifyOfPropertyChange();
            }
        }
    }
}