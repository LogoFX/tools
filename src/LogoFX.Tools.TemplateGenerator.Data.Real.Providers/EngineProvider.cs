using System.Linq;
using Caliburn.Micro;
using LogoFX.Tools.TemplateGenerator.Data.Contracts.Providers;
using LogoFX.Tools.TemplateGenerator.Engine.Contracts;

namespace LogoFX.Tools.TemplateGenerator.Data.Real.Providers
{
    internal sealed class EngineProvider : IEngineProvider
    {
        public ITemplateGeneratorEngine[] GetEngines()
        {
            return IoC.GetAll<ITemplateGeneratorEngine>().ToArray();
        }
    }
}