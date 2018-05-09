using JetBrains.Annotations;
using LogoFX.Client.Mvvm.ViewModel;
using LogoFX.Tools.TemplateGenerator.Data.Contracts;

namespace LogoFX.Tools.TemplateGenerator.Shell.ViewModels
{
    [UsedImplicitly]
    public sealed class SolutionConfigurationPluginViewModel : ObjectViewModel<ISolutionConfigurationPlugin>
    {
        public SolutionConfigurationPluginViewModel(ISolutionConfigurationPlugin model)
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