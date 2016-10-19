using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogoFX.Tools.Common.Model;

namespace LogoFX.Tools.Templates.Wizard.ViewModel
{
    public sealed class SolutionDataViewModel : ObjectViewModel<SolutionData>
    {
        public SolutionDataViewModel(SolutionData model) 
            : base(model)
        {
            Variants = model.Variants.Select(x => new SolutionVariantViewModel(x));
        }

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
            }
        }
    }
}