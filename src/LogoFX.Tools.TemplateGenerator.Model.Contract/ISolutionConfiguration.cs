namespace LogoFX.Tools.TemplateGenerator.Model.Contract
{
    public interface ISolutionConfiguration : IAppModel
    {
        /// <summary>
        /// Name of processing plugin.
        /// </summary>
        string PluginName { get; set; }

        /// <summary>
        /// Solution file name with full path.
        /// </summary>
        string Path { get; }

        /// <summary>
        /// Name of startup project in solution.
        /// </summary>
        string StartupProjectName { get; set; }

        string IconPath { get; set; }

        string PostCreateUrl { get; set; }

        /// <summary>
        /// The default project name in Visual Studio new project dialog.
        /// </summary>
        string DefaultName { get; set; }

        /// <summary>
        /// Output folder.
        /// </summary>
        string TemplateFolder { get; set; }
    }
}