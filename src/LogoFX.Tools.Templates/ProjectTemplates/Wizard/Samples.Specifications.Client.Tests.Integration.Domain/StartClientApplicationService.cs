using LogoFX.Client.Testing.Contracts;
using $saferootprojectname$.Tests.Domain;

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
