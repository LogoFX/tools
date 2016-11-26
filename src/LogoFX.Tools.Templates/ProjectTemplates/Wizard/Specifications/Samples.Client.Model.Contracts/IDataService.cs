using System.Collections.Generic;
using System.Threading.Tasks;

namespace $safeprojectname$
{
    public interface IDataService
    {
        IEnumerable<IWarehouseItem> WarehouseItems { get; }

        Task GetWarehouseItemsAsync();

        Task<IWarehouseItem> NewWarehouseItemAsync();

        void StartEventMonitoring();

        void StopEventMonitoring();

        Task ClearEventsAsync();

        IEnumerable<IEvent> Events { get; }

        bool EventMonitoringStarted { get; }
    }
}
