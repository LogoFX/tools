namespace LogoFX.Tools.TemplateGenerator.Shell.ViewModels
{
    public sealed class WizardConfigurationViewModel : CanGenerateViewModel<WizardConfiguration>
    {
        public WizardConfigurationViewModel(WizardConfiguration model) 
            : base(model)
        {
        }

        public bool IsMultisolution => Model.Solutions.Count > 0;

        public string Name
        {
            get { return Model.Name; }
            set
            {
                if (value == Model.Name)
                {
                    return;
                }

                Model.Name = value;
                NotifyOfPropertyChange();
                OnCanGenerateUpdated();
            }
        }

        public string Description
        {
            get { return Model.Description; }
            set
            {
                if (value == Model.Description)
                {
                    return;
                }

                Model.Description = value;
                NotifyOfPropertyChange();
            }
        }

        public string DefaultName
        {
            get { return Model.DefaultName; }
            set
            {
                if (value == Model.DefaultName)
                {
                    return;
                }

                Model.DefaultName = value;
                NotifyOfPropertyChange();
                OnCanGenerateUpdated();
            }
        }

        protected override bool GetCanGenerate()
        {
            return !string.IsNullOrEmpty(Name) &&
                   !string.IsNullOrEmpty(DefaultName);
        }
    }
}