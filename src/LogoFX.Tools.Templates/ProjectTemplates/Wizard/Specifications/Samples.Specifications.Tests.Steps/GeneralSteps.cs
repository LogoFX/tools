using $saferootprojectname$.Tests.Domain;

namespace $safeprojectname$
{
    public class GeneralSteps
    {
        private readonly IStartClientApplicationService _startClientApplicationService;

        public GeneralSteps(IStartClientApplicationService startClientApplicationService)
        {
            _startClientApplicationService = startClientApplicationService;
        }

        public void WhenIOpenTheApplication()
        {
            _startClientApplicationService.StartApplication();
        }
    }
}