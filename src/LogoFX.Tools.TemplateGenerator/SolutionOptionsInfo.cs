namespace LogoFX.Tools.TemplateGenerator
{
    public sealed class SolutionOptionsInfo
    {
        public SolutionOptionsInfo()
        {
            CanCreateTests = true;
            CanCreateFakes = true;
            CanCreateSamples = true;
            CanSupportNavigation = true;
        }

        public bool CanCreateTests { get; set; }
        public bool DefaultCreateTests { get; set; }
        public bool CanCreateFakes { get; set; }
        public bool DefaultCreateFakes { get; set; }
        public bool CanCreateSamples { get; set; }
        public bool DefaultCreateSamples { get; set; }
        public bool CanSupportNavigation { get; set; }
        public bool DefaultSupportNavigation { get; set; }

        public bool UseOnlyDefautValues { get; set; }
    }
}