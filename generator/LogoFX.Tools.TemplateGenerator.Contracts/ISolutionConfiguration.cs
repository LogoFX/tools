namespace LogoFX.Tools.TemplateGenerator.Contracts
{
    public interface ISolutionConfiguration
    {
        string FileName { get; }
        string DestinationPath { get; set; }
    }
}