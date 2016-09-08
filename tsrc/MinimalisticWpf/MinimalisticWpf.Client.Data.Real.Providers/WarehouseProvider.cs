using System.Collections.Generic;
using JetBrains.Annotations;
using MinimalisticWpf.Client.Data.Contracts.Dto;
using MinimalisticWpf.Client.Data.Contracts.Providers;

namespace MinimalisticWpf.Client.Data.Real.Providers
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