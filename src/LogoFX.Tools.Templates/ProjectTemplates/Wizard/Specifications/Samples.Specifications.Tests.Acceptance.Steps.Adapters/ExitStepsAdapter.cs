using $saferootprojectname$.Tests.Steps;
using TechTalk.SpecFlow;

namespace $safeprojectname$
{
    [Binding]
    class ExitStepsAdapter
    {
        public ExitSteps ExitSteps { get; set; }

        public ExitStepsAdapter(ExitSteps exitSteps)
        {
            ExitSteps = exitSteps;
        }

        [When(@"I select exit with save option")]
        public void WhenISelectExitWithSaveOption()
        {
            ExitSteps.WhenISelectExitWithSaveOption();
        }

        [When(@"I select exit without save option")]
        public void WhenISelectExitWithoutSaveOption()
        {
            ExitSteps.WhenISelectExitWithoutSaveOption();
        }

        [When(@"I select cancel option")]
        public void WhenISelectCancelOption()
        {
            ExitSteps.WhenISelectCancelOption();
        }

        [Then(@"the exit application options are displayed")]
        public void ThenTheExitAppicationOptionsAreDisplayed()
        {
            ExitSteps.ThenTheExitApplicationOptionsDisplayStatusIs(true);
        }

        [Then(@"the exit application options are not displayed")]
        public void ThenTheExitAppicationOptionsAreNotDisplayed()
        {
            ExitSteps.ThenTheExitApplicationOptionsDisplayStatusIs(false);
        }
    }
}
