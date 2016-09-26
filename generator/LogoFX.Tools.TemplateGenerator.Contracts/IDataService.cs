namespace LogoFX.Tools.TemplateGenerator.Contracts
{
    public interface IDataService
    {
        string GetSolutionFileName();

        IConfiguration LoadConfiguration();

        void SaveConfiguration(IConfiguration configuration);

        bool ShowInTaskbar { get; }
    }
}