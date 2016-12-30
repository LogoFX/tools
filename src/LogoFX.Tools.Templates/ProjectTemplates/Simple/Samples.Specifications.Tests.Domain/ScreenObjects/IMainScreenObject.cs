using System.Collections.Generic;
using $saferootprojectname$.Tests.Data;

namespace $safeprojectname$.ScreenObjects
{
    public interface IMainScreenObject
    {
        IEnumerable<WarehouseItemAssertionTestData> GetWarehouseItems();
        WarehouseItemAssertionTestData GetWarehouseItemByKind(string kind);
        bool IsActive();
        void EditWarehouseItem(string kind, string fieldName, string fieldValue);
        void AddWarehouseItem(WarehouseItemAssertionTestData warehouseItemData);
        void DeleteWarehouseItem(string kind);
    }
}