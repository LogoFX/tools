using System.Windows.Input;
using Caliburn.Micro;
using LogoFX.Client.Mvvm.Commanding;
using LogoFX.Client.Mvvm.ViewModel.Extensions;
using LogoFX.Tools.TemplateGenerator.Contracts;

namespace LogoFX.Tools.TemplateGenerator.Shell.ViewModels
{
    public sealed class CreateSolutionViewModel : Screen
    {
        public CreateSolutionViewModel()
        {

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
    }
}