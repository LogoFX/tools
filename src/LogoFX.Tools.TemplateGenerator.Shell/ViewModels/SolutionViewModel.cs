using LogoFX.Client.Mvvm.ViewModel;
using LogoFX.Client.Mvvm.ViewModel.Contracts;
using LogoFX.Tools.TemplateGenerator.Contracts;

namespace LogoFX.Tools.TemplateGenerator.Shell.ViewModels
{
    public sealed class SolutionViewModel : ObjectViewModel<SolutionInfo>, ICanBeBusy
    {
        private readonly bool _multisolution;
        private readonly string _destinationFolder;

        public SolutionViewModel(SolutionInfo model, bool multisolution, string destinationFolder)
            : base(model)
        {
            _multisolution = multisolution;
            _destinationFolder = destinationFolder;
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
                SolutionTemplateInfoGenerator generator = new SolutionTemplateInfoGenerator(_multisolution, _destinationFolder);
                SolutionTemplateInfo = await generator.GenerateTemplateInfoAsync(Model.FileName);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}