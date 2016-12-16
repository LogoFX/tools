using System.Linq;
using $saferootprojectname$.Tests.Data;
using $saferootprojectname$.Tests.Steps;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace $safeprojectname$
{
    [Binding]
    class MainStepsAdapter
    {
        public MainSteps MainSteps { get; set; }

        public MainStepsAdapter(MainSteps mainSteps)
        {
            MainSteps = mainSteps;
        }

        [When(@"I set the Price for ""(.*)"" item to (.*)")]
        public void WhenISetThePriceForItemTo(string kind, int newPrice)
        {
            MainSteps.WhenISetThePriceForItemTo(kind, newPrice);
        }

        [Then(@"I expect to see the following data on the screen:")]
        public void ThenIExpectToSeeTheFollowingDataOnTheScreen(Table table)
        {
            var warehouseItems = table.CreateSet<WarehouseItemAssertionTestData>();
            MainSteps.ThenIExpectToSeeTheFollowingDataOnTheScreen(warehouseItems.ToArray());
        }

        [Then(@"Total cost of ""(.*)"" item is (.*)")]
        public void ThenTotalCostOfItemIs(string kind, int expectedTotalCost)
        {
            MainSteps.ThenTotalCostOfItemIs(kind, expectedTotalCost);
        }

        [When(@"I delete ""(.*)"" item")]
        public void WhenIDeleteItem(string kind)
        {
            MainSteps.WhenIDeleteItem(kind);
        }

        [When(@"I create a new warehouse item with the following data:")]
        public void WhenICreateANewWarehouseItemWithTheFollowingData(Table table)
        {
            var warehouseItems = table.CreateSet<WarehouseItemAssertionTestData>();
            MainSteps.WhenICreateANewWarehouseItemWithTheFollowingData(warehouseItems.ToArray());
        }
    }
}
