namespace LogoFX.Tools.TemplateGenerator.Model.Contract
{
    public interface ISolutionConfiguration : IAppModel
    {
        ISolutionConfigurationPlugin Plugin { get;set; }

        string Path { get; }

        string StartupProjectName { get; set; }
    }
}