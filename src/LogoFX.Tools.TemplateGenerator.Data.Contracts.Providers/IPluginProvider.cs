using LogoFX.Tools.TemplateGenerator.Engine.Contracts;

namespace LogoFX.Tools.TemplateGenerator.Data.Contracts.Providers
{
    public interface IPluginProvider
    {
        ISolutionConfigurationPlugin[] GetPlugins();
    }
}