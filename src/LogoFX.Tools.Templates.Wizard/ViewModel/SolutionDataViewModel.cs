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
    }
}