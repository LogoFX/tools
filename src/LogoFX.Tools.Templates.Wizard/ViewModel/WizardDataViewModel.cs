using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogoFX.Tools.Common.Model;

namespace LogoFX.Tools.Templates.Wizard.ViewModel
{
    public sealed class WizardDataViewModel : ObjectViewModel<WizardData>
    {
        private IEnumerable<SolutionDataViewModel> _solutions;

        public WizardDataViewModel(WizardData model)
            : base(model)
        {
        }

        private string _title;
        public string Title
        {
            get { return _title; }
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

        public IEnumerable<SolutionDataViewModel> Solutions
        {
            get { return _solutions ?? (_solutions = CreateSolutions()); }
        }

        private SolutionDataViewModel _selectedSolution;

        public SolutionDataViewModel SelectedSolution
        {
            get { return _selectedSolution; }
            set
            {
                if (_selectedSolution == value)
                {
                    return;
                }

                SetSelectedSolutionAsync(value);
            }
        }

        private async void SetSelectedSolutionAsync(SolutionDataViewModel value)
        {
            if (value != null)
            {
                SelectedSolution = null;
                await Task.Delay(10);
            }

            _selectedSolution = value;
            NotifyOfPropertyChange(() => SelectedSolution);
            NotifyOfPropertyChange(() => OkEnabled);
        }

        public void SetSelectedSolution(SolutionDataViewModel selectedSolution)
        {
            _selectedSolution = selectedSolution;
        }

        public bool OkEnabled
        {
            get { return SelectedSolution?.SelectedVariant != null; }
        }

        private IEnumerable<SolutionDataViewModel> CreateSolutions()
        {
            return Model.Solutions.Select(x =>
            {
                var solutionDataViewModel = new SolutionDataViewModel(x);
                solutionDataViewModel.SelectedVariantChanged += (sender, args) =>
                {
                    NotifyOfPropertyChange(() => OkEnabled);
                };
                return solutionDataViewModel;
            }).ToList();
        }
    }
}