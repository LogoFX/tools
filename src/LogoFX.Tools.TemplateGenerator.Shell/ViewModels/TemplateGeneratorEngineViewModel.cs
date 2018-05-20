using JetBrains.Annotations;
using LogoFX.Client.Mvvm.ViewModel;
using LogoFX.Tools.TemplateGenerator.Model.Contract;

namespace LogoFX.Tools.TemplateGenerator.Shell.ViewModels
{
    [UsedImplicitly]
    public sealed class TemplateGeneratorEngineViewModel : ObjectViewModel<ITemplateGeneratorEngineInfo>
    {
        public TemplateGeneratorEngineViewModel(ITemplateGeneratorEngineInfo model)
            : base(model)
        {

        }

        public override string DisplayName
        {
            get => Model.Name;
            set { }
        }
    }
}