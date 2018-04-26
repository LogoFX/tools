using System.Linq;
using System.Reflection;
using Solid.Practices.IoC;

namespace $safeprojectname$
{
    public static class ContainerRegistratorExtensions
    {
        public static void RegisterAutomagically(
            this IIocContainerRegistrator @object,
            Assembly contractsAssembly,
            Assembly implementationsAssembly)
        {
            var contracts =
                contractsAssembly.DefinedTypes.Where(t => t.IsInterface).ToArray();
            var implementations =
                implementationsAssembly.DefinedTypes.Where(
                    t => t.IsInterface == false)
                    .ToArray();
            foreach (var contract in contracts)
            {
                var contractName = contract.Name;
                var implementation =
                    implementations.FirstOrDefault(
                        t =>
                            contractName == "I" + t.Name &&
                            contract.Namespace == t.Namespace.Replace("EndToEnd.Domain", "Domain"));
                if (implementation != null)
                {
                    @object.RegisterSingleton(contract, implementation);
                }
            }
        }
    }
}