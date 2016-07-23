using System;
using System.Collections.Generic;
using System.Linq;
using $saferootprojectname$.Tests.Acceptance.Contracts.ScreenObjects;
using $saferootprojectname$.Tests.Acceptance.TestData;
using TestStack.White.InputDevices;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.WindowsAPI;

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
                var cell = match.Cells[fieldName];
                cell.Click();
                cell.Enter(fieldValue);                
                Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);                              
            }
            catch (Exception)
            {                
                throw new InvalidOperationException($"Column {fieldName} cannot be found");
            }                        
        }
    }
}