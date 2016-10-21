using System.Windows.Input;
using Caliburn.Micro;
using LogoFX.Client.Mvvm.Commanding;

namespace LogoFX.Tools.TemplateGenerator.Shell.ViewModels
{
    public sealed class CreateSolutionViewModel : Conductor<SolutionOptionsViewModel>
    {
        private readonly SolutionInfo _solution;

        public CreateSolutionViewModel(SolutionInfo solution)
        {
            _solution = solution;
        }

        private ICommand _okCommand;

        public ICommand OkCommand
        {
            get
            {
                return _okCommand ??
                       (_okCommand = ActionCommand
                           .When(() => !string.IsNullOrEmpty(Name))
                           .Do(() =>
                           {
                               TryClose(true);
                           })
                           .RequeryOnPropertyChanged(this, () => Name));
            }
        }

        private ICommand _cancelCommand;

        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ??
                       (_cancelCommand = ActionCommand
                           .When(() => true)
                           .Do(() =>
                           {
                               TryClose(false);
                           }));
            }
        }

        public override string DisplayName
        {
            get { return "Add new Solution"; }
            set { }
        }

        private string _name;

        /// <summary>
        /// Gets or sets the Solution name.
        /// </summary>
        /// <value>
        /// The Solution name.
        /// </value>
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value)
                {
                    return;
                }

                _name = value;
                NotifyOfPropertyChange();
            }
        }

        private string _caption;

        public string Caption
        {
            get { return _caption; }
            set
            {
                if (_caption == value)
                {
                    return;
                }

                _caption = value;
                NotifyOfPropertyChange();
            }
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            SolutionOptionsInfo options;
            if (_solution == null)
            {
                DisplayName = "Add new solution";
                options = new SolutionOptionsInfo();
            }
            else
            {
                DisplayName = "Edit solution " + _solution.Name;
                options = _solution.Options;
                Name = _solution.Name;
                Caption = _solution.Caption;
            }

            var optionsViewModel = new SolutionOptionsViewModel(options);
            ActivateItem(optionsViewModel);
        }
    }
}