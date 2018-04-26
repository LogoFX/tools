using System;
using $saferootprojectname$.Tests.Domain;
using Solid.Practices.IoC;

namespace $safeprojectname$
{
    public static class Extensions
    {
        public static void Initialize(this IIocContainer iocContainer, Action beforeInitialize)
        {
            new Startup(iocContainer).Initialize(beforeInitialize);
        }

        public static void Setup(this IIocContainer iocContainer)
        {
            var setupService = iocContainer.Resolve<ISetupService>();
            setupService.Setup();
        }
        
        public static void Teardown(this IIocContainer iocContainer)
        {
            var teardownService = iocContainer.Resolve<ITeardownService>();
            teardownService.Teardown();
        }
    }
}