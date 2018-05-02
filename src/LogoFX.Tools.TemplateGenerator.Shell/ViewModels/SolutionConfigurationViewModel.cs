using JetBrains.Annotations;
using LogoFX.Client.Mvvm.ViewModel.Extensions;
using LogoFX.Tools.TemplateGenerator.Model.Contract;

namespace LogoFX.Tools.TemplateGenerator.Shell.ViewModels
{
    [UsedImplicitly]
    public class SolutionConfigurationViewModel : ScreenObjectViewModel<ISolutionConfiguration>
    {
        public SolutionConfigurationViewModel(ISolutionConfiguration model)
            : base(model)
        {

        }
    }
}