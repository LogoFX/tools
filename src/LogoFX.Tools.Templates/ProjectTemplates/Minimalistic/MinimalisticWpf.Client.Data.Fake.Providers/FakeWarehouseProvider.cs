using System;
using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
using $saferootprojectname$.Client.Data.Contracts.Dto;
using $saferootprojectname$.Client.Data.Contracts.Providers;

namespace $safeprojectname$
{
    [UsedImplicitly]
    internal sealed class FakeWarehouseProvider : IWarehouseProvider
    {
        private readonly Random _random = new Random();

        private readonly WarehouseItemDto[] _warehouseItems =
        {
            new WarehouseItemDto
            {
                Kind = "PC",
                Price = 25.43,
                Quantity = 8
            }
        };

        public IEnumerable<WarehouseItemDto> GetWarehouseItems()
        {
            Thread.Sleep(_random.Next(2000));
            return _warehouseItems;
        }
    }
}