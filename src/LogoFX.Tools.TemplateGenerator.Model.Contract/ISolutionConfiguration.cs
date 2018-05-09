namespace LogoFX.Tools.TemplateGenerator.Model.Contract
{
    public interface ISolutionConfiguration : IAppModel
    {
        string PluginName { get; set; }

        string Path { get; }

        string StartupProjectName { get; set; }

        string IconPath { get; set; }

        string PostCreateUrl { get; set; }

        string DefaultName { get; set; }

        string TemplateFolder { get; set; }
    }
}