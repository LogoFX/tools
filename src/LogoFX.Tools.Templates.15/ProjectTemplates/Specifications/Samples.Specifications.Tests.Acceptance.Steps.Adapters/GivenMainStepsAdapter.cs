using System;
using System.Linq;
using $saferootprojectname$.lient.Data.Contracts.Dto;
using $saferootprojectname$.pecifications.Tests.Steps;
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
            var warehouseItems = table.CreateSet<WarehouseItemDto>().ToArray();
            foreach (var warehouseItemDto in warehouseItems)
            {
                warehouseItemDto.Id = Guid.NewGuid();
            }
            GivenMainSteps.SetupWarehouseItems(warehouseItems);
        }
    }
}
