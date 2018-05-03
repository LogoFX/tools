using LogoFX.Client.Mvvm.ViewModel;
using LogoFX.Tools.TemplateGenerator.Model.Contract;

namespace LogoFX.Tools.TemplateGenerator.Shell.ViewModels
{
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