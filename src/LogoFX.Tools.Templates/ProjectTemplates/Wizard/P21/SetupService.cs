using LogoFX.Client.Testing.EndToEnd.White;
using $saferootprojectname$.Tests.Domain;

namespace $safeprojectname$
{
    class SetupService : ISetupService
    {
        public void Setup()
        {
            
        }
    }

    class TeardownService : ITeardownService
    {
        public void Teardown()
        {
            ApplicationContext.Application?.Dispose();
        }
    }
}
