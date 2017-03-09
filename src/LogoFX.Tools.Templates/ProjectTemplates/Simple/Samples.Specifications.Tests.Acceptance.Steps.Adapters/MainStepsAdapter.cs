using $saferootprojectname$.Tests.Steps;
using TechTalk.SpecFlow;

namespace $safeprojectname$
{
    [Binding]
    class MainStepsAdapter
    {
        public MainSteps MainSteps { get; set; }

        public MainStepsAdapter(
            MainSteps mainSteps)
        {
            MainSteps = mainSteps;
        }

        [Then(@"Application navigates to the main screen")]
        public void ThenApplicationNavigatesToTheMainScreen()
        {
            //for readability reasons
        }
    }
}
