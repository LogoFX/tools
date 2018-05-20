using JetBrains.Annotations;
using LogoFX.Tools.TemplateGenerator.Engine.Contracts;

namespace LogoFX.Tools.TemplateGenerator.Engine.SamplesSpecification
{
    [UsedImplicitly]
    internal sealed class TemplateGeneratorEngine : ITemplateGeneratorEngine
    {
        public string Name => "Samples.Specification";
    }
}