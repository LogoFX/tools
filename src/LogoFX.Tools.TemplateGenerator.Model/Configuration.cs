using LogoFX.Tools.TemplateGenerator.Model.Contract;

namespace LogoFX.Tools.TemplateGenerator.Model
{
    internal sealed class Configuration : IConfiguration
    {
        public ISolutionConfiguration[] Solutions { get; }
    }
}