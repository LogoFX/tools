using $saferootprojectname$.pecifications.Tests.Steps;
using TechTalk.SpecFlow;

namespace $safeprojectname$
{
    [Binding]
    class GeneralStepsAdapter
    {
        public GeneralSteps GeneralSteps { get; set; }

        public GeneralStepsAdapter(GeneralSteps generalSteps)
        {
            GeneralSteps = generalSteps;
        }

        [When(@"I open the application")]
        public void WhenIOpenTheApplication()
        {
            GeneralSteps.WhenIOpenTheApplication();
        }

        [When(@"I close the application")]
        public void WhenICloseTheApplication()
        {
            GeneralSteps.WhenICloseTheApplication();
        }

        [When(@"I wait for (.*) seconds")]
        public void WhenIWaitForSeconds(int seconds)
        {
            GeneralSteps.WaitFor(seconds);
        }
    }
}
