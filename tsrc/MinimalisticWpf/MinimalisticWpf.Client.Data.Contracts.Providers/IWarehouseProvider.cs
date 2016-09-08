using System.Collections.Generic;
using MinimalisticWpf.Client.Data.Contracts.Dto;

namespace MinimalisticWpf.Client.Data.Contracts.Providers
{
    public interface IWarehouseProvider
    {
        IEnumerable<WarehouseItemDto> GetWarehouseItems();
    }
}