using JetBrains.Annotations;
using LogoFX.Client.Mvvm.ViewModel;
using LogoFX.Tools.TemplateGenerator.Engine.Contracts;

namespace LogoFX.Tools.TemplateGenerator.Shell.ViewModels
{
    [UsedImplicitly]
    public sealed class TemplateGeneratorEngineViewModel : ObjectViewModel<ITemplateGeneratorEngine>
    {
        public TemplateGeneratorEngineViewModel(ITemplateGeneratorEngine model)
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