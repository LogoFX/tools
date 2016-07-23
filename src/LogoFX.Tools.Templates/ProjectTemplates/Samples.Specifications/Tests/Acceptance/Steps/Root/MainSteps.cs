using System;
using System.Linq;
using FluentAssertions;
using $saferootprojectname$.Tests.Acceptance.Contracts.ScreenObjects;
using $saferootprojectname$.Tests.Acceptance.TestData;

namespace $safeprojectname$
{
    public class MainSteps
    {
        private readonly IMainScreenObject _mainScreenObject;

        public MainSteps(IMainScreenObject mainScreenObject)
        {
            _mainScreenObject = mainScreenObject;
        }

        public void WhenISetThePriceForItemTo(string kind, int newPrice)
        {
            _mainScreenObject.EditWarehouseItem(kind, "Price", newPrice.ToString());
        }

        public void ThenIExpectToSeeTheFollowingDataOnTheScreen(WarehouseItemAssertionTestData[] warehouseItems)
        {
            var actualWarehouseItems = _mainScreenObject.GetWarehouseItems().ToArray();
            for (int i = 0; i < Math.Max(warehouseItems.Length, actualWarehouseItems.Length); i++)
            {
                var expectedWarehouseItem = warehouseItems[i];
                var actualWarehouseItem = actualWarehouseItems[i];
                actualWarehouseItem.Kind.Should().Be(expectedWarehouseItem.Kind);
                actualWarehouseItem.Price.Should().Be(expectedWarehouseItem.Price);
                actualWarehouseItem.Quantity.Should().Be(expectedWarehouseItem.Quantity);
                actualWarehouseItem.TotalCost.Should().Be(expectedWarehouseItem.TotalCost);               
            }            
        }

        public void ThenTotalCostOfItemIs(string kind, int expectedTotalCost)
        {
            var actualWarehouseItem = _mainScreenObject.GetWarehouseItemByKind(kind);
            actualWarehouseItem.TotalCost.Should().Be(expectedTotalCost);
        }
    }
}
