using $saferootprojectname$.Client.Data.Contracts.Dto;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace $safeprojectname$
{
    [Binding]
    class GivenMainStepsAdapter
    {
        public GivenMainSteps GivenMainSteps { get; set; }

        public GivenMainStepsAdapter(GivenMainSteps givenMainSteps)
        {
            GivenMainSteps = givenMainSteps;
        }

        [Given(@"warehouse contains the following items:")]
        public void GivenWarehouseContainsTheFollowingItems(Table table)
        {
            var warehouseItems = table.CreateSet<WarehouseItemDto>();
            GivenMainSteps.SetupWarehouseItems(warehouseItems);
        }
    }
}
