using JetBrains.Annotations;
using LogoFX.Tools.TemplateGenerator.Engine.Contracts;
using Solid.Practices.IoC;
using Solid.Practices.Modularity;

namespace LogoFX.Tools.TemplateGenerator.Engine.Services
{
    [UsedImplicitly]
    internal sealed class Module : ICompositionModule<IDependencyRegistrator>
    {
        public void RegisterModule(IDependencyRegistrator dependencyRegistrator)
        {
            dependencyRegistrator.RegisterSingleton<ITemplateGeneratorService, TemplateGeneratorService>();
        }
    }
}