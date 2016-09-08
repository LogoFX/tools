using System.Collections.Generic;
using System.Threading.Tasks;

namespace MinimalisticWpf.Client.Model.Contracts
{
    public interface IDataService
    {
        IEnumerable<IWarehouseItem> WarehouseItems { get; }

        Task GetWarehouseItemsAsync();
    }
}