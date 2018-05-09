using JetBrains.Annotations;
using LogoFX.Tools.TemplateGenerator.Data.Contracts;

namespace LogoFX.Tools.TemplateGenerator.Plugin.SamplesSpecification
{
    [UsedImplicitly]
    internal sealed class SolutionConfigurationPlugin : ISolutionConfigurationPlugin
    {
        public string Name => "Samples.Specification";
    }
}