using System.Collections.Generic;
using System.Linq;
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

        private bool _createFakes;

        public bool CreateFakes
        {
            get { return _createFakes; }
            set
            {
                if (_createFakes == value)
                {
                    return;
                }

                _createFakes = value;
                NotifyOfPropertyChange();
            }
        }

        private bool _createTests;

        public bool CreateTests
        {
            get { return _createTests; }
            set
            {
                if (_createTests == value)
                {
                    return;
                }

                _createTests = value;
                NotifyOfPropertyChange();
            }
        }

        public bool MustRemoveConditions
        {
            get { return !CreateFakes || !CreateTests; }
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

                _selectedSolution = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => OkEnabled);
            }
        }

        public bool OkEnabled
        {
            get { return SelectedSolution != null; }
        }

        private IEnumerable<SolutionDataViewModel> CreateSolutions()
        {
            return Model.Solutions.Select(x => new SolutionDataViewModel(x)).ToList();
        }
    }
}