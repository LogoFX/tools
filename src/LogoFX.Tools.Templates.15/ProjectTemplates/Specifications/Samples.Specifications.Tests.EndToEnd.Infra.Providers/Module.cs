using System;
using System.Linq;
using Attest.Fake.Registration;
using JetBrains.Annotations;
using LogoFX.Client.Testing.EndToEnd.FakeData.Modularity;
using LogoFX.Client.Testing.EndToEnd.FakeData.Shared;
using $saferootprojectname$.pecifications.Client.Data.Fake.Shared;
using Solid.Patterns.Builder;
using Solid.Practices.IoC;

namespace $safeprojectname$
{    
    [UsedImplicitly]
    public class Module : ProvidersModuleBase
    {       
        protected override void OnRegisterProviders(IDependencyRegistrator dependencyRegistrator)
        {
            base.OnRegisterProviders(dependencyRegistrator);            
            var typeMatches = Helper.FindProviderMatches();            
            foreach (var typeMatch in typeMatches)
            {
                var instance = Helper.CreateInstance(typeMatch.Key);
                RegisterAllBuildersInternal(dependencyRegistrator, (IBuilder)instance, typeMatch.Key, typeMatch.Value);
            }            
        }

        private static void RegisterAllBuildersInternal(IDependencyRegistrator dependencyRegistrator,
            IBuilder builderInstance, Type builderType, Type providerType)
        {
            var builders = BuildersCollectionContext.GetBuilders(builderType).OfType<IBuilder>().ToArray();
            if (builders.Length == 0)
            {
                RegistrationHelper.RegisterBuilder(dependencyRegistrator, providerType, builderInstance);                
            }
            else
            {
                foreach (var builder in builders)
                {
                    RegistrationHelper.RegisterBuilder(dependencyRegistrator, providerType, builder);
                }
            }
        }        
    }    
}
