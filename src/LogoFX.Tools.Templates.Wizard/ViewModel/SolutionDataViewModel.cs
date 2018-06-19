using LogoFX.Tools.Common.Model;

namespace LogoFX.Tools.Templates.Wizard.ViewModel
{
    public sealed class SolutionDataViewModel : ObjectViewModel<SolutionData>
    {
        public SolutionDataViewModel(SolutionData model) 
            : base(model)
        {

        }

        public bool MustRemoveConditions => !CreateFakes || !CreateTests;

        public bool CanCreateTests => CreateSamples;

        private bool _createTests;

        public bool CreateTests
        {
            get => _createTests;
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

        public bool CanCreateFakes => true;

        private bool _createFakes;

        public bool CreateFakes
        {
            get => _createFakes;
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

        public bool CanCreateSamples => true;

        private bool _createSamples;

        public bool CreateSamples
        {
            get => _createSamples;
            set
            {
                if (_createSamples == value)
                {
                    return;
                }

                _createSamples = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => CanCreateTests);

                if (!CreateSamples)
                {
                    CreateTests = false;                   
                }
            }
        }
    }
}