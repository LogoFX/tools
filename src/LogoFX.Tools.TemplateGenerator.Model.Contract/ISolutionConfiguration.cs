namespace LogoFX.Tools.TemplateGenerator.Model.Contract
{
    public interface ISolutionConfiguration : IAppModel
    {
        /// <summary>
        /// Name of processing engine.
        /// </summary>
        string EngineName { get; set; }

        /// <summary>
        /// Solution file name with full path.
        /// </summary>
        string Path { get; }

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