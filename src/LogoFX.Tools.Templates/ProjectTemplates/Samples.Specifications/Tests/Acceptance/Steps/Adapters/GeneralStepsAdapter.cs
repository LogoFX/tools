using $saferootprojectname$.Tests.Acceptance.Steps.Platform;
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
    }
}
