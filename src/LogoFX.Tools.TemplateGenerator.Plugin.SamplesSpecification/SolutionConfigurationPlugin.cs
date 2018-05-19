using JetBrains.Annotations;
using LogoFX.Tools.TemplateGenerator.Engine.Contracts;

namespace LogoFX.Tools.TemplateGenerator.Plugin.SamplesSpecification
{
    [UsedImplicitly]
    internal sealed class SolutionConfigurationPlugin : ISolutionConfigurationPlugin
    {
        public string Name => "Samples.Specification";

        private TemplateGeneratorEngine _engine;
        public ITemplateGeneratorEngine Engine => _engine ?? (_engine = new TemplateGeneratorEngine());
    }
}