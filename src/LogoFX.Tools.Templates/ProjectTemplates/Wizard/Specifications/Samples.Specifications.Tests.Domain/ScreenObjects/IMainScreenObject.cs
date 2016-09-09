using System.Collections.Generic;
using $saferootprojectname$.Tests.Data;

namespace $safeprojectname$.ScreenObjects
{
    public interface IMainScreenObject
    {
        IEnumerable<WarehouseItemAssertionTestData> GetWarehouseItems();
        WarehouseItemAssertionTestData GetWarehouseItemByKind(string kind);
        void EditWarehouseItem(string kind, string fieldName, string fieldValue);
        bool IsActive();
    }
}