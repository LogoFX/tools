namespace LogoFX.Tools.TemplateGenerator.Engine.Contracts
{
    public interface ISolutionConfigurationPlugin
    {
        string Name { get; }

        ITemplateGeneratorEngine Engine { get; }
    }
}