using LogoFX.Tools.Common.Model;

namespace LogoFX.Tools.Templates.Wizard.ViewModel
{
    public sealed class SolutionVariantViewModel : ObjectViewModel<SolutionVariantData>
    {
        public SolutionVariantViewModel(SolutionVariantData model) 
            : base(model)
        {
        }

        public string ContainerName
        {
            get { return Model.ContainerName; }
        }
    }
}