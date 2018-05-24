using System.Reflection;
using $saferootprojectname$.pecifications.Tests.Domain;
using Solid.Practices.IoC;
using Solid.Practices.Modularity;

namespace $safeprojectname$
{
    class Module : ICompositionModule<IDependencyRegistrator>
    {
        public void RegisterModule(IDependencyRegistrator dependencyRegistrator)
        {
            dependencyRegistrator.RegisterAutomagically(Assembly.LoadFrom("$saferootprojectname$.pecifications.Tests.Domain.dll"),
                Assembly.GetExecutingAssembly());
        }
    }
}
