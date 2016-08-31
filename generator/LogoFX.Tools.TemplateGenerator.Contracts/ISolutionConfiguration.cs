namespace LogoFX.Tools.TemplateGenerator.Contracts
{
    public interface ISolutionConfiguration
    {
        string FileName { get; }
        string DestinationPath { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        string DefaultName { get; set; }
    }
}