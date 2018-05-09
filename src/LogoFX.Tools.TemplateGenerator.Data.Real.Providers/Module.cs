using JetBrains.Annotations;
using LogoFX.Tools.TemplateGenerator.Data.Contracts.Providers;
using Solid.Practices.IoC;
using Solid.Practices.Modularity;

namespace LogoFX.Tools.TemplateGenerator.Data.Real.Providers
{
    [UsedImplicitly]
    internal sealed class Module : ICompositionModule<IDependencyRegistrator>
    {
        public void RegisterModule(IDependencyRegistrator dependencyRegistrator)
        {
            dependencyRegistrator.AddSingleton<IConfigurationProvider, ConfigurationProvider>();
            dependencyRegistrator.AddSingleton<IPluginProvider, PluginProvider>();
        }
    }
}