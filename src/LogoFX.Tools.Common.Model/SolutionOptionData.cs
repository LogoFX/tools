namespace LogoFX.Tools.Common.Model
{
    public class SolutionOptionData
    {
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