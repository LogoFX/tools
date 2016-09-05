namespace LogoFX.Tools.TemplateGenerator.Shell.ViewModels
{
    public sealed class WizardConfigurationViewModel : ObjectViewModel<WizardConfiguration>
    {
        public WizardConfigurationViewModel(WizardConfiguration model) 
            : base(model)
        {
        }

        public bool IsMultisolution => Model.Solutions.Count > 0;
    }
}