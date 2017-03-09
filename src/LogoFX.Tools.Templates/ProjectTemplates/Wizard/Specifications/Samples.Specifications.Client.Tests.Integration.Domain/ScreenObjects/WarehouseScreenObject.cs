using System;
using System.Collections.Generic;
using System.Linq;
using $saferootprojectname$.Client.Presentation.Shell.ViewModels;
using $saferootprojectname$.Client.Tests.Integration.Infra.Core;
using $saferootprojectname$.Tests.Data;
using $saferootprojectname$.Tests.Domain.ScreenObjects;

namespace $safeprojectname$.ScreenObjects
{
    class WarehouseScreenObject : IWarehouseScreenObject
    {
        public StructureHelper StructureHelper { get; }

        public WarehouseScreenObject(StructureHelper structureHelper)
        {
            StructureHelper = structureHelper;
        }       

        public void AddWarehouseItem(WarehouseItemAssertionTestData warehouseItemData)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteWarehouseItem(string kind)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<WarehouseItemAssertionTestData> GetWarehouseItems()
        {
            var main = StructureHelper.GetMain();
            return main.WarehouseItems.Items.OfType<WarehouseItemViewModel>()
                .Select(t => new WarehouseItemAssertionTestData
                {
                    Kind = t.Model.Kind,
                    Price = t.Model.Price,
                    Quantity = t.Model.Quantity,
                    TotalCost = t.Model.TotalCost
                });
        }

        public WarehouseItemAssertionTestData GetWarehouseItemByKind(string kind)
        {
            var main = StructureHelper.GetMain();
            return
                main.WarehouseItems.Items.OfType<WarehouseItemViewModel>()
                    .Where(t => t.Model.Kind == kind)
                    .Select(t => new WarehouseItemAssertionTestData
                    {
                        Kind = t.Model.Kind,
                        Price = t.Model.Price,
                        Quantity = t.Model.Quantity,
                        TotalCost = t.Model.TotalCost
                    }).Single();
        }

        public void EditWarehouseItem(string kind, string newKind, double? newPrice, int? newQuantity)
        {
            var main = StructureHelper.GetMain();
            var itemViewModel =
                main.WarehouseItems.Items
                    .OfType<WarehouseItemViewModel>().Single(t => t.Model.Kind == kind);

            if (newPrice != null)
            {
                itemViewModel.Model.Price = newPrice.Value;
            }

            if (newQuantity != null)
            {
                itemViewModel.Model.Quantity = newQuantity.Value;
            }
        }

        public string GetErrorMessage()
        {
            throw new System.NotImplementedException();
        }

        public void DiscardChanges()
        {
            throw new System.NotImplementedException();
        }

        public Tuple<bool, bool> AreStatusIndicatorsEnabled()
        {
            throw new System.NotImplementedException();
        }
    }

    class MainScreenObject : IMainScreenObject
    {
        public StructureHelper StructureHelper { get; set; }

        public MainScreenObject(StructureHelper structureHelper)
        {
            StructureHelper = structureHelper;
        }

        public bool IsActive()
        {
            var main = StructureHelper.GetMain();
            return main.IsActive;
        }
    }
}