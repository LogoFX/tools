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

        //TODO: make async
        void StartEventMonitoring();

        //TODO: make async
        void StopEventMonitoring();

        Task ClearEventsAsync();

        IEnumerable<IEvent> Events { get; }

        bool EventMonitoringStarted { get; }
    }
}
