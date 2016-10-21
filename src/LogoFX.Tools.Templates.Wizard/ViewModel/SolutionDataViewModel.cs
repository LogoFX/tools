using System;
using System.Collections.Generic;
using System.Linq;
using LogoFX.Tools.Common.Model;

namespace LogoFX.Tools.Templates.Wizard.ViewModel
{
    public sealed class SolutionDataViewModel : ObjectViewModel<SolutionData>
    {
        public SolutionDataViewModel(SolutionData model) 
            : base(model)
        {
            Variants = Model.Variants.Select(x => new SolutionVariantViewModel(x)).ToList();

            CreateTests = Model.Options.DefaultCreateTests;
            CreateFakes = Model.Options.DefaultCreateFakes;
            CreateSamples = Model.Options.DefaultCreateSamples;
            SupportNavigation = Model.Options.DefaultSupportNavigation;
        }

        public EventHandler SelectedVariantChanged = delegate { };

        public IEnumerable<SolutionVariantViewModel> Variants { get; private set; }

        private SolutionVariantViewModel _selectedVariant;

        public SolutionVariantViewModel SelectedVariant
        {
            get { return _selectedVariant; }
            set
            {
                if (_selectedVariant == value)
                {
                    return;
                }

                _selectedVariant = value;
                NotifyOfPropertyChange();

                SelectedVariantChanged(this, EventArgs.Empty);
            }
        }
        public bool MustRemoveConditions
        {
            get { return !CreateFakes || !CreateTests; }
        }

        public bool CanCreateTests => Model.Options.CanCreateTests;

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

        public bool CanCreateFakes => Model.Options.CanCreateFakes;

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

        public bool CanCreateSamples => Model.Options.CanCreateSamples;

        private bool _createSamples;

        public bool CreateSamples
        {
            get { return _createSamples; }
            set
            {
                if (_createSamples == value)
                {
                    return;
                }

                _createSamples = value;
                NotifyOfPropertyChange();
            }
        }

        public bool CanSupportNavigation => Model.Options.CanSupportNavigation;

        private bool _supportNavigation;

        public bool SupportNavigation
        {
            get { return _supportNavigation; }
            set
            {
                if (_supportNavigation == value)
                {
                    return;
                }

                _supportNavigation = value;
                NotifyOfPropertyChange();
            }
        }

        public bool UseOnlyDefautValues => Model.Options.UseOnlyDefautValues;
    }
}