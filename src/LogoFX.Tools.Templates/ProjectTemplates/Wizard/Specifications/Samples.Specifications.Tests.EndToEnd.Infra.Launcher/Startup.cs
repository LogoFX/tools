using System;
using LogoFX.Bootstrapping;
using Solid.Practices.IoC;

namespace $safeprojectname$
{    
    class Startup
    {
        private readonly IIocContainer _iocContainer;

        public Startup(IIocContainer iocContainer)
        {
            _iocContainer = iocContainer;                   
        }

        public void Initialize(Action beforeInitialize)
        {
            var bootstrapper =
                new Bootstrapper(_iocContainer)
                    .Use(new RegisterCompositionModulesMiddleware<Bootstrapper>());
            beforeInitialize();                 
            bootstrapper.Initialize();
        }
    }
}
