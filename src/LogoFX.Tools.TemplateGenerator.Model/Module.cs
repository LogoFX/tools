using JetBrains.Annotations;
using LogoFX.Tools.TemplateGenerator.Model.Contract;
using Solid.Practices.IoC;
using Solid.Practices.Modularity;

namespace LogoFX.Tools.TemplateGenerator.Model
{
    [UsedImplicitly]
    class Module : ICompositionModule<IDependencyRegistrator>
    {
        public void RegisterModule(IDependencyRegistrator dependencyRegistrator)
        {
            dependencyRegistrator.AddSingleton<IDataService, DataService>();
        }
    }
}