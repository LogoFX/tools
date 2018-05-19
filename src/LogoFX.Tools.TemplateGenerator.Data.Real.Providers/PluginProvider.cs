using System.Linq;
using Caliburn.Micro;
using LogoFX.Tools.TemplateGenerator.Data.Contracts.Providers;
using LogoFX.Tools.TemplateGenerator.Engine.Contracts;

namespace LogoFX.Tools.TemplateGenerator.Data.Real.Providers
{
    internal sealed class PluginProvider : IPluginProvider
    {
        public ISolutionConfigurationPlugin[] GetPlugins()
        {
            return IoC.GetAll<ISolutionConfigurationPlugin>().ToArray();
        }
    }
}