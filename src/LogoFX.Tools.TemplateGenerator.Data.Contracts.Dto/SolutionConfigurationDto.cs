using JetBrains.Annotations;

namespace LogoFX.Tools.TemplateGenerator.Data.Contracts.Dto
{
    [UsedImplicitly]
    public sealed class SolutionConfigurationDto : BaseAppDto
    {
        public string EngineName { get; set; }
        public string Path { get; set; }
        public string IconPath { get; set; }
        public string PostCreateUrl { get; set; }
        public string DefaultName { get; set; }
        public string TemplateFolder { get; set; }
    }
}