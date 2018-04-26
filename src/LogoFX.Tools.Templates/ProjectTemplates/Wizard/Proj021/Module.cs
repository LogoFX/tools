using System.Reflection;
using Solid.Practices.IoC;
using Solid.Practices.Modularity;

namespace $safeprojectname$
{
    class Module : ICompositionModule<IIocContainerRegistrator>
    {
        public void RegisterModule(IIocContainerRegistrator iocContainer)
        {            
            iocContainer.RegisterAutomagically(Assembly.LoadFrom("$saferootprojectname$.Tests.Domain.dll"),
                Assembly.GetExecutingAssembly());
            iocContainer.RegisterSingleton<IExecutableContainer, ExecutableContainer>();
            iocContainer.RegisterSingleton<StructureHelper, StructureHelper>();
        }
    }
}
