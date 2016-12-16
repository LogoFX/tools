using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using $saferootprojectname$.Tests.Data;
using $saferootprojectname$.Tests.Domain.ScreenObjects;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;

namespace $safeprojectname$.ScreenObjects
{
    class MainScreenObject : IMainScreenObject
    {
        public IEnumerable<WarehouseItemAssertionTestData> GetWarehouseItems()
        {
            var shell = StructureHelper.GetShell();
            var dataGrid = shell.Get<ListView>(SearchCriteria.ByAutomationId("WarehouseItemsDataGrid"));
            return dataGrid.Rows.Select(CreateWarehouseItemAssertionTestData);            
        }

        public WarehouseItemAssertionTestData GetWarehouseItemByKind(string kind)
        {
            var match = GetRowByKind(kind);
            return CreateWarehouseItemAssertionTestData(match);
        }

        private ListViewRow GetRowByKind(string kind)
        {
            var shell = StructureHelper.GetShell();
            var dataGrid = shell.Get<ListView>(SearchCriteria.ByAutomationId("WarehouseItemsDataGrid"));
            var match = dataGrid.Rows.Find(t => t.Cells["Kind"].Text == kind);
            if (match == null)
            {
                throw new InvalidOperationException($"Warehouse item {kind} cannot be found");
            }
            return match;
        }

        private static WarehouseItemAssertionTestData CreateWarehouseItemAssertionTestData(ListViewRow t)
        {
            return new WarehouseItemAssertionTestData
            {
                Kind = t.Cells["Kind"].Text,                    
                Price = double.Parse(t.Cells["Price"].Text),
                Quantity = int.Parse(t.Cells["Quantity"].Text),
                TotalCost = double.Parse(t.Cells["Total cost"].Text)
            };
        }

        public void EditWarehouseItem(string kind, string fieldName, string fieldValue)
        {           
            var match = GetRowByKind(kind);
            try
            {
                match.Focus();
                Task.Delay(1000).Wait();
                match.Cells[0].Click();
            }
            catch (Exception err)
            {
                throw new InvalidOperationException($"Item {kind} cannot be found", err);
            }

            var shell = StructureHelper.GetShell();
            var priceTextBox = shell.Get<TextBox>(SearchCriteria.ByAutomationId("WarehouseItemPriceTextBox"));
            priceTextBox.Enter(fieldValue);
        }

        public bool IsActive()
        {
            //for nows
            return true;
        }

        public void DeleteWarehouseItem(string kind)
        {
            var match = GetRowByKind(kind);
            try
            {
                match.Focus();
                Task.Delay(1000).Wait();
                match.Cells[0].Click();
            }
            catch (Exception err)
            {
                throw new InvalidOperationException($"Item {kind} cannot be found", err);
            }

            var shell = StructureHelper.GetShell();
            var deleteButton = shell.Get<Button>(SearchCriteria.ByAutomationId("WarehouseItemDeleteButton"));
            deleteButton.Click();
        }

        public void AddWarehouseItem(WarehouseItemAssertionTestData warehouseItemData)
        {
            var shell = StructureHelper.GetShell();

            var deleteButton = shell.Get<Button>(SearchCriteria.ByAutomationId("WarehouseItemNewButton"));
            deleteButton.Click();

            var kindTextBox = shell.Get<TextBox>(SearchCriteria.ByAutomationId("WarehouseItemKindTextBox"));
            var priceTextBox = shell.Get<TextBox>(SearchCriteria.ByAutomationId("WarehouseItemPriceTextBox"));
            var quantityTextBox = shell.Get<TextBox>(SearchCriteria.ByAutomationId("WarehouseItemQuantityTextBox"));

            kindTextBox.Enter(warehouseItemData.Kind);
            priceTextBox.BulkText = warehouseItemData.Price.ToString(CultureInfo.CurrentCulture);
            quantityTextBox.BulkText = warehouseItemData.Quantity.ToString(CultureInfo.CurrentCulture);

            var applyButton = shell.Get<Button>(SearchCriteria.ByAutomationId("WarehouseItemApplyButton"));
            applyButton.Click();
        }
    }
}