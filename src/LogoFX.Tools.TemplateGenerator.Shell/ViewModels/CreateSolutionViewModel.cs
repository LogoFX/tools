using LogoFX.Client.Mvvm.ViewModel.Extensions;
using LogoFX.Tools.TemplateGenerator.Contracts;

namespace LogoFX.Tools.TemplateGenerator.Shell.ViewModels
{
    public sealed class CreateSolutionViewModel : ScreenObjectViewModel<SolutionInfo>
    {
        public CreateSolutionViewModel(SolutionInfo model)
            : base(model)
        {

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
    }


}