using System.Collections.Generic;
using JetBrains.Annotations;
using $saferootprojectname$.Client.Data.Contracts.Dto;
using $saferootprojectname$.Client.Data.Contracts.Providers;

namespace $safeprojectname$
{
    [UsedImplicitly]
    public sealed class WarehouseProvider : IWarehouseProvider
    {
        public IEnumerable<WarehouseItemDto> GetWarehouseItems()
        {
            throw new System.NotImplementedException();
        }
    }
}