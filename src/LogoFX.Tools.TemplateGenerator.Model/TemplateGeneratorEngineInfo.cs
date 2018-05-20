using JetBrains.Annotations;
using LogoFX.Tools.TemplateGenerator.Model.Contract;

namespace LogoFX.Tools.TemplateGenerator.Model
{
    [UsedImplicitly]
    internal sealed class TemplateGeneratorEngineInfo : ITemplateGeneratorEngineInfo
    {
        [UsedImplicitly]
        public string Name { get; private set; }
    }
}