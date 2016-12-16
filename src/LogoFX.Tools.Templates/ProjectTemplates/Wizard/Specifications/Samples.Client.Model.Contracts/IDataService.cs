using System.Collections.Generic;
using System.Threading.Tasks;

namespace $safeprojectname$
{
    public interface IDataService
    {
        IEnumerable<IWarehouseItem> WarehouseItems { get; }

        Task GetWarehouseItemsAsync();

        Task<IWarehouseItem> NewWarehouseItemAsync();

        Task SaveWarehouseItemAsync(IWarehouseItem item);

        Task DeleteWarehouseItemAsync(IWarehouseItem item);

        void StartEventMonitoring();

        void StopEventMonitoring();

        Task ClearEventsAsync();

        IEnumerable<IEvent> Events { get; }

        bool EventMonitoringStarted { get; }
    }
}
