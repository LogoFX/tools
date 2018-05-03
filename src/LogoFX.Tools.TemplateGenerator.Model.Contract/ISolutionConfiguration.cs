namespace LogoFX.Tools.TemplateGenerator.Model.Contract
{
    public interface ISolutionConfiguration : IAppModel
    {
        ISolutionConfigurationPlugin Plugin { get;set; }

        string Path { get; }

        string StartupProjectName { get; set; }

        string IconPath { get; set; }

        string PostCreateUrl { get; set; }

        string DefaultName { get; set; }

        string TemplateFolder { get; set; }
    }
}