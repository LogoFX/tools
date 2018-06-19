using LogoFX.Tools.Common.Model;

namespace LogoFX.Tools.Templates.Wizard.ViewModel
{
    public sealed class WizardDataViewModel : ObjectViewModel<WizardData>
    {
        public WizardDataViewModel(WizardData model)
            : base(model)
        {
        }

        public string Title
        {
            get => _title;
            set
            {
                if (value == _title)
                {
                    return;
                }

                _title = value;
                NotifyOfPropertyChange();
            }
        }

        private SolutionDataViewModel _solution;
        private string _title;
        public SolutionDataViewModel Solution => _solution ?? (_solution = new SolutionDataViewModel(Model.Solution));
    }
}