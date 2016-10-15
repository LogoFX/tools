using LogoFX.Client.Mvvm.ViewModel;
using LogoFX.Tools.TemplateGenerator.Contracts;

namespace LogoFX.Tools.TemplateGenerator.Shell.ViewModels
{
    public sealed class ProjectViewModel : ObjectViewModel<IProjectTemplateInfo>
    {
        public ProjectViewModel(IProjectTemplateInfo model)
            : base(model)
        {

        }

        public string Name
        {
            get { return Model.NameWithoutRoot; }
        }

        public bool IsStartup
        {
            get { return Model.IsStartup; }
            set
            {
                if (IsStartup == value)
                {
                    return;
                }

                Model.IsStartup = value;
                NotifyOfPropertyChange();
            }
        }
    }
}