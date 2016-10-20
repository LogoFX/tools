namespace LogoFX.Tools.TemplateGenerator
{
    public sealed class SolutionInfo
    {
        public SolutionInfo()
        {
            SolutionVariants = new SolutionVariant[0];
            Options = new SolutionOptionsInfo();
        }

        public string Name { get; set; }
        public string Caption { get; set; }
        public string IconName { get; set; }
        public string PostCreateUrl { get; set; }
        public SolutionOptionsInfo Options { get; set; }
        public SolutionVariant[] SolutionVariants { get; set; }
    }
}