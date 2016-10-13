namespace LogoFX.Tools.TemplateGenerator.Contracts
{
    public interface IProjectConfiguration
    {
        string ConfigurationName { get; }
        string PlatformName { get; set; }
        string Name { get; set; }
        bool IncludeInBuild { get; set; }
    }
}