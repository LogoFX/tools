using System;
using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
using MinimalisticWpf.Client.Data.Contracts.Dto;
using MinimalisticWpf.Client.Data.Contracts.Providers;

namespace MinimalisticWpf.Client.Data.Fake.Providers
{
    [UsedImplicitly]
    public sealed class FakeWarehouseProvider : IWarehouseProvider
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