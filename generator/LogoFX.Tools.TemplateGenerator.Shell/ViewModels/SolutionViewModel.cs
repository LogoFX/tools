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

        private ISolutionTemplateInfo _solutionTemplateInfo;

        public ISolutionTemplateInfo SolutionTemplateInfo
        {
            get { return _solutionTemplateInfo; }
            private set
            {
                if (_solutionTemplateInfo == value)
                {
                    return;
                }

                _solutionTemplateInfo = value;
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