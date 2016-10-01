using LogoFX.Client.Mvvm.ViewModel;
using LogoFX.Client.Mvvm.ViewModel.Contracts;
using LogoFX.Tools.TemplateGenerator.Contracts;

namespace LogoFX.Tools.TemplateGenerator.Shell.ViewModels
{
    public sealed class SolutionViewModel : ObjectViewModel<SolutionInfo>, ICanBeBusy
    {
        public SolutionViewModel(SolutionInfo model)
            : base(model)
        {
            CreateSolutionTemplateInfo();
        }

        public ISolutionTemplateInfo SolutionTemplateInfo
        {
            get { return Model.SolutionTemplateInfo; }
            private set
            {
                if (SolutionTemplateInfo == value)
                {
                    return;
                }

                Model.SolutionTemplateInfo = value;
                NotifyOfPropertyChange();
            }
        }

        private async void CreateSolutionTemplateInfo()
        {
            IsBusy = true;

            try
            {
                SolutionTemplateInfoGenerator generator = new SolutionTemplateInfoGenerator();
                SolutionTemplateInfo = await generator.GenerateTemplateInfoAsync(Model.FileName);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}