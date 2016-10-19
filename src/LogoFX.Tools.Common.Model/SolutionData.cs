namespace LogoFX.Tools.Common.Model
{
    public class SolutionData
    {
        public string Name { get; set; }

        public string Caption { get; set; }

        public string IconFileName { get; set; }

        public string PostCreateUrl { get; set; }

        public SolutionVariantData[] Variants { get; set; }

        public SolutionOptionData Options { get; set; }
    }
}