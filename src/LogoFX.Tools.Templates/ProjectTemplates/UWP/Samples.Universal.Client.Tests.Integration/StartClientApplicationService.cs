using LogoFX.Client.Testing.Contracts;
using $saferootprojectname$.Client.Tests.Contracts;

namespace $safeprojectname$
{
    class StartClientApplicationService : IStartClientApplicationService
    {
        private readonly IStartApplicationService _startApplicationService;

        public StartClientApplicationService(IStartApplicationService startApplicationService)
        {
            _startApplicationService = startApplicationService;
        }

        public void StartApplication()
        {
            _startApplicationService.StartApplication(string.Empty);
        }
    }
}
