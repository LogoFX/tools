using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using LogoFX.Tools.TemplateGenerator;

namespace LogoFX.Tools.Templates.Wizard.ViewModel
{
    public sealed class WizardViewModel : ObjectViewModel<WizardConfiguration>
    {
        private IEnumerable<SolutionInfoViewModel> _solutions;

        public WizardViewModel(WizardConfiguration model)
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

        public bool FakeOption
        {
            get { return Model.FakeOption; }
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

        public bool TestOption
        {
            get { return Model.TestOption; }
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

        public IEnumerable<SolutionInfoViewModel> Solutions
        {
            get { return _solutions ?? (_solutions = CreateSolutions()); }
        }

        private SolutionInfoViewModel _selectedSolution;

        public SolutionInfoViewModel SelectedSolution
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

        private IEnumerable<SolutionInfoViewModel> CreateSolutions()
        {
            return Model.Solutions.Select(x => new SolutionInfoViewModel(x)).ToList();
        }
    }
}