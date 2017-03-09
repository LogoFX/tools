using System.Linq;
using $saferootprojectname$.Tests.Data;
using $saferootprojectname$.Tests.Steps;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace $safeprojectname$
{
    [Binding]
    class WarehouseStepsAdapter
    {
        public WarehouseSteps WarehouseSteps { get; set; }

        public WarehouseStepsAdapter(WarehouseSteps warehouseSteps)
        {
            WarehouseSteps = warehouseSteps;
        }

        [When(@"I set the Price for ""(.*)"" item to (.*)")]
        public void WhenISetThePriceForItemTo(string kind, int newPrice)
        {
            WarehouseSteps.WhenISetThePriceForItemTo(kind, newPrice);
        }

        [When(@"I set the Quantity for ""(.*)"" item to (.*)")]
        public void WhenISetTheQuantityForItemTo(string kind, int newQuantity)
        {
            WarehouseSteps.WhenISetTheQuantityForItemTo(kind, newQuantity);
        }

        [When(@"I set the Kind for ""(.*)"" item to ""(.*)""")]
        public void WhenISetTheKindForItemTo(string kind, string newKind)
        {
            WarehouseSteps.WhenISetTheKindForItemTo(kind, newKind);
        }

        [When(@"I discard the changes")]
        public void WhenIDiscardTheChanges()
        {
            WarehouseSteps.WhenIDiscardTheChanges();
        }

        [Then(@"I expect to see the following data on the screen:")]
        public void ThenIExpectToSeeTheFollowingDataOnTheScreen(Table table)
        {
            var warehouseItems = table.CreateSet<WarehouseItemAssertionTestData>();
            WarehouseSteps.ThenIExpectToSeeTheFollowingDataOnTheScreen(warehouseItems.ToArray());
        }

        [Then(@"Total cost of ""(.*)"" item is (.*)")]
        public void ThenTotalCostOfItemIs(string kind, int expectedTotalCost)
        {
            WarehouseSteps.ThenTotalCostOfItemIs(kind, expectedTotalCost);
        }

        [When(@"I delete ""(.*)"" item")]
        public void WhenIDeleteItem(string kind)
        {
            WarehouseSteps.WhenIDeleteItem(kind);
        }

        [When(@"I create a new warehouse item with the following data:")]
        public void WhenICreateANewWarehouseItemWithTheFollowingData(Table table)
        {
            var warehouseItems = table.CreateSet<WarehouseItemAssertionTestData>();
            WarehouseSteps.WhenICreateANewWarehouseItemWithTheFollowingData(warehouseItems.ToArray());
        }

        [Then(@"Error message is displayed with the following text ""(.*)""")]
        public void ThenErrorMessageIsDisplayedWithTheFollowingText(string expectedErrorMessage)
        {
            WarehouseSteps.ThenErrorMessageIsDisplayedWithTheFollowingText(expectedErrorMessage);
        }

        [Then(@"the Price for ""(.*)"" item is (.*)")]
        public void ThenThePriceForItemIs(string kind, int price)
        {
            WarehouseSteps.ThenThePriceForItemIs(kind, price);
        }

        [Then(@"the Quantity for ""(.*)"" item is (.*)")]
        public void ThenTheQuantityForItemIs(string kind, int quantity)
        {
            WarehouseSteps.ThenTheQuantityForItemIs(kind, quantity);
        }

        [Then(@"the changes are saved")]
        public void ThenTheChangesAreSaved()
        {
            WarehouseSteps.ThenTheChangesAreSaved();
        }

        [Then(@"the changes are discarded")]
        public void ThenTheChangesAreDiscarded()
        {
            WarehouseSteps.ThenTheChangesAreDiscarded();
        }

        [Then(@"the changes are intact")]
        public void ThenTheChangesAreIntact()
        {
            WarehouseSteps.ThenTheChangesAreIntact();
        }
    }
}